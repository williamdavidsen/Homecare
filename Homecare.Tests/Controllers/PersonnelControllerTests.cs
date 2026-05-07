using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Homecare.Controllers;
using Homecare.DAL.Interfaces;
using Homecare.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Homecare.Tests.Controllers
{
    public class PersonnelControllerTests
    {
        private static DefaultHttpContext Ctx()
        {
            var ctx = new DefaultHttpContext();
            return ctx;
        }

        private static void AttachTempData(Controller c)
        {
            c.ControllerContext = new ControllerContext { HttpContext = Ctx() };
            c.TempData = new TempDataDictionary(c.HttpContext, Mock.Of<ITempDataProvider>());
        }

        [Fact]
        public async Task CreateDay_NEGATIVE_BlockedDays_SetTempDataError_And_Redirect()
        {
            // arrange
            const int pid = 2;
            var from = DateOnly.FromDateTime(DateTime.Today);
            var to = from.AddDays(42);

            var existing = new List<DateOnly>
            {
                from.AddDays(1), // This day will stay selected.
                from.AddDays(2)  // This day will be removed.
            };

            var locked = new List<DateOnly> { from.AddDays(2) }; // Prevent removal.

            var slotRepo = new Mock<IAvailableSlotRepository>();
            slotRepo.Setup(r => r.GetWorkDaysAsync(pid, It.IsAny<DateOnly>(), It.IsAny<DateOnly>()))
                    .ReturnsAsync(existing);
            slotRepo.Setup(r => r.GetLockedDaysAsync(pid, It.IsAny<DateOnly>(), It.IsAny<DateOnly>()))
                    .ReturnsAsync(locked);

            // RemoveRange will not be called because the day is locked.
            slotRepo.Setup(r => r.GetSlotsForPersonnelOnDayAsync(pid, It.IsAny<DateOnly>()))
                    .ReturnsAsync(new List<AvailableSlot>());
            slotRepo.Setup(r => r.RemoveRangeAsync(It.IsAny<IEnumerable<AvailableSlot>>()))
                    .Returns(Task.CompletedTask);

            slotRepo.Setup(r => r.AddRangeAsync(It.IsAny<IEnumerable<AvailableSlot>>()))
                    .Returns(Task.CompletedTask);

            var apptRepo = new Mock<IAppointmentRepository>();
            var userRepo = new Mock<IUserRepository>();

            var sut = new PersonnelController(
                apptRepo.Object,
                slotRepo.Object,
                userRepo.Object,
                Mock.Of<ILogger<PersonnelController>>());
            AttachTempData(sut);

            // Selected days: only existing[0], so one day.
            var daysCsv = string.Join(",", existing[0].ToString("yyyy-MM-dd"));

            // act
            var result = await sut.CreateDay(pid, daysCsv);

            // assert
            var rd = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(PersonnelController.Dashboard), rd.ActionName);
            Assert.Equal(pid, rd.RouteValues!["personnelId"]);

            Assert.True(sut.TempData.ContainsKey("Error"));   // Blocked message.
        }

        [Fact]
        public async Task CreateDay_POSITIVE_AddsThreeSlotsPerChosenDay_And_SetsSuccessMessage()
        {
            // arrange
            const int pid = 3;
            var from = DateOnly.FromDateTime(DateTime.Today).AddDays(1);
            var chosen = new[] { from, from.AddDays(1) };
            var existing = new List<DateOnly>();     // None.
            var locked = new List<DateOnly>();     // engel yok

            var slotRepo = new Mock<IAvailableSlotRepository>();
            slotRepo.Setup(r => r.GetWorkDaysAsync(pid, It.IsAny<DateOnly>(), It.IsAny<DateOnly>()))
                    .ReturnsAsync(existing);
            slotRepo.Setup(r => r.GetLockedDaysAsync(pid, It.IsAny<DateOnly>(), It.IsAny<DateOnly>()))
                    .ReturnsAsync(locked);

            var added = new List<AvailableSlot>();
            slotRepo.Setup(r => r.AddRangeAsync(It.IsAny<IEnumerable<AvailableSlot>>()))
                    .Callback<IEnumerable<AvailableSlot>>(lst => added.AddRange(lst))
                    .Returns(Task.CompletedTask);

            // No removal expected.
            slotRepo.Setup(r => r.RemoveRangeAsync(It.IsAny<IEnumerable<AvailableSlot>>()))
                    .Returns(Task.CompletedTask);

            var apptRepo = new Mock<IAppointmentRepository>();
            var userRepo = new Mock<IUserRepository>();

            var sut = new PersonnelController(
                apptRepo.Object,
                slotRepo.Object,
                userRepo.Object,
                Mock.Of<ILogger<PersonnelController>>());
            AttachTempData(sut);

            var csv = string.Join(",", chosen.Select(d => d.ToString("yyyy-MM-dd")));

            // act
            var result = await sut.CreateDay(pid, csv);

            // assert
            var rd = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(PersonnelController.Dashboard), rd.ActionName);
            Assert.Equal(pid, rd.RouteValues!["personnelId"]);

            // Three preset slots for each day.
            Assert.Equal(chosen.Length * 3, added.Count);
            Assert.True(sut.TempData.ContainsKey("Message"));
        }
    }
}
