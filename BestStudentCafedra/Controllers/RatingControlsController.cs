﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BestStudentCafedra.Data;
using BestStudentCafedra.Models;
using BestStudentCafedra.Models.ViewModels;
using System.Text;
using ClosedXML.Excel;
using System.IO;
using System.Globalization;
using System.ComponentModel;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BestStudentCafedra.Controllers
{
    public class RatingControlsController : Controller
    {
        public static string EXAMTYPENAME = "Экзамен";
        private readonly SubjectAreaDbContext _context;

        public RatingControlsController(SubjectAreaDbContext context)
        {
            _context = context;
        }

        // GET: RatingControls/Details/5
        public async Task<IActionResult> Group(int? id, int? disciplineId)
        {
            if (id == null || disciplineId == null)
            {
                return NotFound();
            }

            var groupRating = new GroupRatingViewModel();
            groupRating.RatingControls = await _context.RatingControls
                .Include(x => x.StudentRatings)
                .Where(x => x.SemesterDisciplineId == disciplineId && x.GroupId == id)
                .OrderBy(x => x.Number)
                .ToListAsync();

            groupRating.Group = await _context.AcademicGroups
                .Include(x => x.Specialty)
                .Include(x => x.Students.OrderBy(y => y.FullName))
                    .ThenInclude(x => x.ActivityProtections
                        .Where(y => _context.Activities
                            .Where(z => z.SemesterDisciplineId == disciplineId)
                            .Any(h => y.ActivityId == h.Id)))
                .FirstOrDefaultAsync(x => x.Id == id);

            groupRating.SemesterDiscipline = await _context.SemesterDiscipline
                .Include(x => x.Discipline)
                .Include(x => x.Activities)
                    .ThenInclude(y => y.Type)
                .FirstOrDefaultAsync(x => x.Id == disciplineId);

            return View(groupRating);
        }

        // GET: AcademicGroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rating = await _context.RatingControls
                .Include(s => s.StudentRatings)
                .ThenInclude(s => s.Student)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (rating == null)
            {
                return NotFound();
            }

            rating.StudentRatings = rating.StudentRatings.OrderBy(x => x.Student.FullName).ToList();
            return View(rating);
        }

        // GET: RatingControls/Create
        public IActionResult Create(int? groupId, int? disciplineId)
        {
            if (groupId == null || disciplineId == null)
            {
                return NotFound();
            }

            var ratingControl = new RatingControl();
            ratingControl.GroupId = (int)groupId;
            ratingControl.SemesterDisciplineId = (int)disciplineId;

            return PartialView("_Create", ratingControl);
        }

        // POST: RatingControls/CreateConfirm
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConfirm(int? groupId, int? disciplineId)
        {
            if (groupId == null || disciplineId == null)
            {
                return NotFound();
            }

            var lastNumber = 0;
            if (_context.RatingControls.Count() > 0)
            {
                var groupRatingControls = _context.RatingControls.Where(x => x.GroupId == groupId && x.SemesterDisciplineId == disciplineId);
                if (groupRatingControls != null) lastNumber = groupRatingControls.OrderByDescending(x => x.Number).FirstOrDefault().Number;
            }

            var newRatingControl = new RatingControl();
            newRatingControl.Number = lastNumber + 1;
            newRatingControl.GroupId = (int)groupId;
            newRatingControl.SemesterDisciplineId = (int)disciplineId;
            newRatingControl.CompletionDate = DateTime.Now;

            _context.Add(newRatingControl);
            await _context.SaveChangesAsync();

            var students = await _context.Students
                .Where(x => x.GroupId == groupId)
                .ToListAsync();

            var activityProtections = await _context.Activities
                .Where(x => x.SemesterDisciplineId == disciplineId)
                .Include(x => x.ActivityProtections.Where(y => y.Student.GroupId == groupId))
                .SelectMany(x => x.ActivityProtections)
                .ToListAsync();


            var studentsRating = new List<StudentRating>();
            foreach (var student in students)
            {
                var studentRating = new StudentRating();
                studentRating.RatingId = newRatingControl.Id;
                studentRating.StudentId = student.GradebookNumber;
                studentRating.Points = activityProtections
                    .Where(x => x.StudentId == student.GradebookNumber)
                    .Select(x => x.Points)
                    .Sum();

                studentsRating.Add(studentRating);
            }

            _context.AddRange(studentsRating);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Group), new { id = groupId, disciplineId = disciplineId });
        }

        // GET: RatingControls/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ratingControl = await _context.RatingControls.FindAsync(id);
            if (ratingControl == null)
            {
                return NotFound();
            }

            return PartialView("_Edit", ratingControl);
        }

        // POST: RatingControls/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,SemesterDisciplineId,GroupId,Number,CompletionDate")] RatingControl ratingControl)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ratingControl);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RatingControlExists(ratingControl.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return RedirectToAction(nameof(Details), new { id = ratingControl.Id });
        }

        // GET: RatingControls/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ratingControl = await _context.RatingControls
                .Include(r => r.SemesterDiscipline)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ratingControl == null)
            {
                return NotFound();
            }

            return PartialView("_Delete", ratingControl);
        }

        // POST: RatingControls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ratingControl = await _context.RatingControls
                .Include(x => x.StudentRatings)
                .FirstOrDefaultAsync(x => x.Id == id);

            _context.StudentRatings.RemoveRange(ratingControl.StudentRatings);
            _context.RatingControls.Remove(ratingControl);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Group), new { id = ratingControl.GroupId, disciplineId = ratingControl.SemesterDisciplineId });
        }

        public async Task<IActionResult> DownloadRating(int id)
        {
            var ratingControl = await _context.RatingControls
                .Include(x => x.SemesterDiscipline)
                    .ThenInclude(y => y.Discipline)
                        .ThenInclude(z => z.TeacherDisciplines)
                            .ThenInclude(h => h.Teacher)
                .Include(x => x.AcademicGroup)
                    .ThenInclude(y => y.Specialty)
                .Include(x => x.StudentRatings)
                    .ThenInclude(y => y.Student)
                .FirstOrDefaultAsync(x => x.Id == id);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Ведомость");
                var currentRow = 1;

                var width = 14;
                worksheet.Column(2).Width = width;
                worksheet.Column(3).Width = width;
                worksheet.Column(4).Width = width;
                worksheet.Column(5).Width = width;
                worksheet.Column(6).Width = width;

                worksheet.Cell(currentRow, 2).Value = "Промежуточный рейтинг (Рубежный контроль)"; currentRow++;
                worksheet.Cell(currentRow, 2).Value = "Номер"; worksheet.Cell(currentRow, 3).Value = ratingControl.Number;
                worksheet.Cell(currentRow, 5).Value = "Дата:"; worksheet.Cell(currentRow, 6).Value = ratingControl.CompletionDate.Date; currentRow++;
                currentRow++;

                worksheet.Cell(currentRow, 2).Value = "Группа:"; worksheet.Cell(currentRow, 3).Value = ratingControl.AcademicGroup.Name;
                worksheet.Cell(currentRow, 3).Style.Font.Bold = true; currentRow++;
                worksheet.Cell(currentRow, 2).Value = "Код:"; worksheet.Cell(currentRow, 3).Value = $"'{ratingControl.AcademicGroup.Specialty.Code}";
                worksheet.Cell(currentRow, 5).Value = "Специальность:"; worksheet.Cell(currentRow, 6).Value = ratingControl.AcademicGroup.Specialty.Name;
                worksheet.Cell(currentRow, 6).Style.Alignment.WrapText = true; worksheet.Row(currentRow).Height = 15; currentRow++;
                currentRow++;

                worksheet.Cell(currentRow, 2).Value = "Дисциплина:"; worksheet.Cell(currentRow, 3).Value = ratingControl.SemesterDiscipline.Discipline.Name;
                worksheet.Range(worksheet.Cell(currentRow, 3), worksheet.Cell(currentRow, 6)).Merge(); worksheet.Cell(currentRow, 3).Style.Font.Bold = true; currentRow++;
                worksheet.Cell(currentRow, 2).Value = "Курс:"; worksheet.Cell(currentRow, 3).Value = ratingControl.SemesterDiscipline.Year;
                worksheet.Cell(currentRow, 5).Value = "Семестр:"; worksheet.Cell(currentRow, 6).Value = ratingControl.SemesterDiscipline.Semester; currentRow++;
                currentRow++;

                worksheet.Cell(currentRow, 2).Value = "Преподаватель:"; worksheet.Cell(currentRow, 3).Value = ratingControl.SemesterDiscipline.Discipline.TeacherDisciplines.FirstOrDefault(x => x.DisciplineId == ratingControl.SemesterDiscipline.DisciplineId).Teacher.FullName;
                worksheet.Range(worksheet.Cell(currentRow, 3), worksheet.Cell(currentRow, 6)).Merge(); worksheet.Cell(currentRow, 3).Style.Font.Bold = true; currentRow++;
                currentRow++;

                worksheet.Cell(currentRow, 1).Value = "#"; worksheet.Cell(currentRow, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(currentRow, 2).Value = "Зач. книжка"; worksheet.Cell(currentRow, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(currentRow, 3).Value = "ФИО";
                worksheet.Cell(currentRow, 6).Value = "Баллы"; worksheet.Cell(currentRow, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                worksheet.Range(worksheet.Cell(currentRow, 3), worksheet.Cell(currentRow, 5)).Merge();
                worksheet.Range(worksheet.Cell(currentRow, 3), worksheet.Cell(currentRow, 5)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Range(worksheet.Cell(currentRow, 1), worksheet.Cell(currentRow, 6)).Style.Font.Bold = true;
                currentRow++;

                var i = 1;
                foreach (var studentRating in ratingControl.StudentRatings)
                {
                    worksheet.Cell(currentRow, 1).Value = i; worksheet.Cell(currentRow, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(currentRow, 2).Value = studentRating.Student.GradebookNumber; worksheet.Cell(currentRow, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(currentRow, 3).Value = studentRating.Student.FullName;
                    worksheet.Range(worksheet.Cell(currentRow, 3), worksheet.Cell(currentRow, 5)).Merge(); worksheet.Range(worksheet.Cell(currentRow, 3), worksheet.Cell(currentRow, 5)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(currentRow, 6).Value = studentRating.Points; worksheet.Cell(currentRow, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    currentRow++;
                    i++;
                }
                worksheet.Column(1).AdjustToContents();

                worksheet.Cell(currentRow, 1).Value = "Подписанную и сканированную копию ведомости необходимо сдать в деканат"; currentRow++;

                currentRow++;
                worksheet.Cell(currentRow, 2).Value = "Подпись";
                worksheet.Cell(currentRow, 3).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{ratingControl.AcademicGroup.Name} рейтинг №{ratingControl.Number}.xlsx");
                }
            }
        }

        public async Task<IActionResult> DownloadCurrentRating(int groupId, int disciplineId)
        {
            var ratingControls = await _context.RatingControls
                .Include(x => x.StudentRatings)
                .Where(x => x.SemesterDisciplineId == disciplineId && x.GroupId == groupId)
                .OrderBy(x => x.Number)
                .ToListAsync();

            var group = await _context.AcademicGroups
                .Include(x => x.Specialty)
                .Include(x => x.Students.OrderBy(y => y.FullName))
                    .ThenInclude(x => x.ActivityProtections
                        .Where(y => _context.Activities
                            .Where(z => z.SemesterDisciplineId == disciplineId)
                            .Any(h => y.ActivityId == h.Id)))
                .FirstOrDefaultAsync(x => x.Id == groupId);

            var semesterDiscipline = await _context.SemesterDiscipline
                .Include(x => x.Discipline)
                    .ThenInclude(y => y.TeacherDisciplines)
                        .ThenInclude(z => z.Teacher)
                .Include(x => x.Activities)
                    .ThenInclude(y => y.Type)
                .FirstOrDefaultAsync(x => x.Id == disciplineId);

            var pointMultiplier = 0;
            if (semesterDiscipline.ControlType == ControlType.Exam)
            {
                pointMultiplier = (int)(60 / semesterDiscipline.Activities.Where(x => x.Type.Name != EXAMTYPENAME).Select(x => x.MaxPoints).Sum());
            }
            else
            {
                pointMultiplier = (int)(100 / semesterDiscipline.Activities.Select(x => x.MaxPoints).Sum());
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Текущий рейтинг контроль");
                var currentRow = 1;

                worksheet.Cell(currentRow, 1).Value = "гр.";
                worksheet.Cell(currentRow, 2).Value = group.Name; worksheet.Cell(currentRow, 2).Style.Fill.BackgroundColor = XLColor.FromArgb(254, 0, 254);
                worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 3).Value = "Расчетная ведомость рейтинга:";
                worksheet.Range(worksheet.Cell(currentRow, 3), worksheet.Cell(currentRow, 3 + 5)).Merge();
                worksheet.Cell(currentRow, 3 + 5 + 1).Value = semesterDiscipline.Discipline.Name;
                currentRow++;

                worksheet.Cell(currentRow, 1).Value = "сп.";
                worksheet.Cell(currentRow, 2).Value = group.Specialty.Name; worksheet.Cell(currentRow, 2).Style.Fill.BackgroundColor = XLColor.FromArgb(254, 0, 254);

                var currentCol = 3;
                var r = new Random();
                foreach (var activityType in semesterDiscipline.Activities.Select(x => x.Type).Distinct())
                {
                    var length = semesterDiscipline.Activities.Where(x => x.TypeId == activityType.Id).Count() - 1;
                    worksheet.Range(worksheet.Cell(currentRow, currentCol), worksheet.Cell(currentRow, currentCol + length)).Merge();
                    worksheet.Cell(currentRow, currentCol).Value = Abbreviaturization(activityType.Name);
                    worksheet.Cell(currentRow, currentCol).Style.Font.Bold = true;
                    worksheet.Cell(currentRow, currentCol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    worksheet.Range(worksheet.Cell(currentRow + 1, currentCol), worksheet.Cell(currentRow + 2 + group.Students.Count(), currentCol + length)).Style.Fill.BackgroundColor = XLColor.FromArgb(100 + r.Next(155), 100 + r.Next(155), 100 + r.Next(155));
                    currentCol += length + 1;
                }
                currentRow++;

                currentCol = 3;
                foreach (var item in semesterDiscipline.Activities)
                {
                    worksheet.Cell(currentRow, currentCol).Value = $"№{item.Number}";
                    worksheet.Cell(currentRow + 1, currentCol).Value = item.MaxPoints * pointMultiplier;
                    worksheet.Cell(currentRow + 1, currentCol).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    worksheet.Column(currentCol).Width = 4;
                    currentCol++;
                }

                worksheet.Cell(currentRow, currentCol).Value = "Итого";

                worksheet.Cell(currentRow, currentCol).Style.Font.Bold = true;
                worksheet.Cell(currentRow, currentCol).Style.Alignment.TextRotation = 90;
                worksheet.Cell(currentRow, currentCol).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(currentRow, currentCol).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                worksheet.Column(currentCol).Width = 6;
                worksheet.Range(worksheet.Cell(currentRow + 2, currentCol), worksheet.Cell(currentRow + 2 + group.Students.Count(), currentCol)).Style.Fill.BackgroundColor = XLColor.FromArgb(51, 102, 255);
                
                worksheet.Cell(currentRow + 1, currentCol).Value = 100;
                worksheet.Cell(currentRow + 1, currentCol).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(currentRow + 1, currentCol).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(currentRow + 1, currentCol).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                currentCol++;

                foreach (var item in ratingControls)
                {
                    worksheet.Cell(currentRow - 1, currentCol).Value = item.Number + " рейтинг";
                    worksheet.Cell(currentRow - 1, currentCol).Style.Font.FontSize = 8;
                    worksheet.Cell(currentRow, currentCol).Value = item.CompletionDate.Date;
                    worksheet.Cell(currentRow, currentCol).Style.Alignment.TextRotation = 90;
                    worksheet.Cell(currentRow, currentCol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(currentRow, currentCol).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Cell(currentRow, currentCol).Style.Font.FontSize = 9;
                    worksheet.Cell(currentRow, currentCol).Style.Fill.BackgroundColor = XLColor.FromArgb(254, 0, 254);
                    worksheet.Cell(currentRow + 1, currentCol).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(currentRow + 1, currentCol).Value = $"#{item.Number}";
                    worksheet.Cell(currentRow + 1, currentCol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(currentRow + 1, currentCol).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Column(currentCol).Width = 5;
                    currentCol++;
                }

                if (semesterDiscipline.ControlType == ControlType.Exam)
                {
                    if (semesterDiscipline.Activities.FirstOrDefault(x => x.Type.Name == EXAMTYPENAME) != null)
                    {
                        worksheet.Cell(currentRow, currentCol).Value = EXAMTYPENAME;
                        worksheet.Cell(currentRow, currentCol).Style.Alignment.TextRotation = 90;
                        worksheet.Cell(currentRow, currentCol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        worksheet.Cell(currentRow, currentCol).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        worksheet.Cell(currentRow, currentCol).Style.Font.FontSize = 9;
                        worksheet.Cell(currentRow, currentCol).Style.Fill.BackgroundColor = XLColor.FromArgb(254, 0, 254);

                        worksheet.Cell(currentRow + 1, currentCol).Value = "ЭКЗ";
                        worksheet.Cell(currentRow + 1, currentCol).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        worksheet.Cell(currentRow + 1, currentCol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        worksheet.Cell(currentRow + 1, currentCol).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                        worksheet.Column(currentCol).Width = 5;
                        currentCol++;
                    }
                }

                worksheet.Cell(currentRow, currentCol).Value = "Оценка";
                worksheet.Cell(currentRow, currentCol).Style.Alignment.TextRotation = 90;
                worksheet.Cell(currentRow, currentCol).Style.Font.Bold = true;
                worksheet.Cell(currentRow, currentCol).Style.Border.LeftBorder = XLBorderStyleValues.Double;
                worksheet.Cell(currentRow, currentCol).Style.Border.RightBorder = XLBorderStyleValues.Double;

                if (semesterDiscipline.ControlType == ControlType.Exam)
                {
                    worksheet.Cell(currentRow + 1, currentCol).Value = 5;

                    worksheet.Range(worksheet.Cell(currentRow + 2, currentCol), worksheet.Cell(currentRow + 2 + group.Students.Count(), currentCol))
                        .AddConditionalFormat().WhenEquals("5").Fill.SetBackgroundColor(XLColor.FromArgb(0, 176, 80));
                    worksheet.Range(worksheet.Cell(currentRow + 2, currentCol), worksheet.Cell(currentRow + 2 + group.Students.Count(), currentCol))
                        .AddConditionalFormat().WhenEquals("4").Fill.SetBackgroundColor(XLColor.FromArgb(255, 245, 60));
                    worksheet.Range(worksheet.Cell(currentRow + 2, currentCol), worksheet.Cell(currentRow + 2 + group.Students.Count(), currentCol))
                        .AddConditionalFormat().WhenEquals("3").Fill.SetBackgroundColor(XLColor.FromArgb(255, 80, 80));
                }
                else
                {
                    worksheet.Cell(currentRow + 1, currentCol).Value = "Зач";
                    worksheet.Range(worksheet.Cell(currentRow + 2, currentCol), worksheet.Cell(currentRow + 2 + group.Students.Count(), currentCol))
                       .AddConditionalFormat().WhenEquals("Зач").Fill.SetBackgroundColor(XLColor.FromArgb(0, 176, 80));
                }

                worksheet.Cell(currentRow + 1, currentCol).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(currentRow + 1, currentCol).Style.Border.LeftBorder = XLBorderStyleValues.Double;
                worksheet.Cell(currentRow + 1, currentCol).Style.Border.RightBorder = XLBorderStyleValues.Double;

                worksheet.Column(currentCol).Width = 5;
                currentCol++;

                currentCol++; 
                var startCol = currentCol;  currentRow--;
                foreach (var activityType in semesterDiscipline.Activities.Select(x => x.Type).Distinct())
                {
                    var length = semesterDiscipline.Activities.Where(x => x.TypeId == activityType.Id).Count() - 1;
                    worksheet.Range(worksheet.Cell(currentRow, currentCol), worksheet.Cell(currentRow, currentCol + length)).Merge();
                    worksheet.Cell(currentRow, currentCol).Value = Abbreviaturization(activityType.Name);
                    worksheet.Cell(currentRow, currentCol).Style.Font.Bold = true;
                    worksheet.Cell(currentRow, currentCol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    worksheet.Range(worksheet.Cell(currentRow + 1, currentCol), worksheet.Cell(currentRow + 2 + group.Students.Count(), currentCol + length)).Style.Fill.BackgroundColor = XLColor.FromArgb(100 + r.Next(155), 100 + r.Next(155), 100 + r.Next(155));
                    currentCol += length + 1;
                }
                currentRow++;  currentCol = startCol;

                foreach (var item in semesterDiscipline.Activities)
                {
                    worksheet.Cell(currentRow, currentCol).Value = $"№{item.Number}";
                    worksheet.Cell(currentRow + 1, currentCol).Value = item.MaxPoints;
                    worksheet.Cell(currentRow + 1, currentCol).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    worksheet.Column(currentCol).Width = 4;
                    currentCol++;
                }

                worksheet.Row(currentRow).Height = 50;
                currentRow++;

                worksheet.Cell(currentRow, 1).Value = "№"; worksheet.Cell(currentRow, 1).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(currentRow, 2).Value = "Ф.И.О. студента"; worksheet.Cell(currentRow, 2).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                worksheet.Row(currentRow).Height = 20;
                currentRow++;

                var i = 1;
                foreach (var student in group.Students)
                {
                    worksheet.Cell(currentRow, 1).Value = i++;
                    worksheet.Cell(currentRow, 2).Value = student.FullName;
                    worksheet.Cell(currentRow, 2).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                    currentCol = 3;
                    foreach (var activity in semesterDiscipline.Activities)
                    {
                        ActivityProtection activityProtection = student.ActivityProtections.FirstOrDefault(x => x.Activity == activity);
                        if (activityProtection != null)
                        {
                            if (activityProtection.Activity.Type.Name == EXAMTYPENAME)
                                worksheet.Cell(currentRow, currentCol).Value = (40 / activityProtection.Activity.MaxPoints) * activityProtection.Points;
                            else
                                worksheet.Cell(currentRow, currentCol).Value = activityProtection.Points * pointMultiplier;
                        }
                        currentCol++;
                    }

                    worksheet.Cell(currentRow, currentCol).FormulaA1 = $"SUM({worksheet.Cell(currentRow, currentCol - 1).Address}:{worksheet.Cell(currentRow, currentCol - semesterDiscipline.Activities.Count).Address})";
                    worksheet.Cell(currentRow, currentCol).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(currentRow, currentCol).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(currentRow, currentCol).Style.Font.Bold = true;
                    var totalCol = currentCol;
                    currentCol++;

                    foreach (var ratingControl in ratingControls)
                    {
                        StudentRating studentRating = ratingControl.StudentRatings.FirstOrDefault(x => x.StudentId == student.GradebookNumber);
                        if (studentRating != null)
                        {
                            worksheet.Cell(currentRow, currentCol).Value = studentRating.Points * pointMultiplier;
                            worksheet.Cell(currentRow, currentCol).Style.Font.Bold = true;
                        }
                        currentCol++;
                    }

                    if (semesterDiscipline.ControlType == ControlType.Exam)
                    {
                        var exam = student.ActivityProtections.OrderBy(x => x.Points).Where(x => x.Activity.Type.Name == EXAMTYPENAME).FirstOrDefault();
                        worksheet.Cell(currentRow, currentCol).Value = (exam != null) ? (40 / exam.Activity.MaxPoints) * exam.Points : 0;
                        worksheet.Cell(currentRow, currentCol).Style.Font.Bold = true;
                        currentCol++;
                    }

                    var totalCell = worksheet.Cell(currentRow, totalCol).Address;
                    worksheet.Cell(currentRow, currentCol).Style.Border.LeftBorder = XLBorderStyleValues.Double;
                    worksheet.Cell(currentRow, currentCol).Style.Border.RightBorder = XLBorderStyleValues.Double;
                    worksheet.Cell(currentRow, currentCol).Style.Font.Bold = true;

                    if (semesterDiscipline.ControlType == ControlType.Exam)
                        worksheet.Cell(currentRow, currentCol).FormulaA1 = $"IF({totalCell}>90,5,IF({totalCell}>73,4,IF({totalCell}>60,3,2)))";
                    else
                        worksheet.Cell(currentRow, currentCol).FormulaA1 = $"IF({totalCell}>60,\"Зач\",\"Нез\")";
                    currentCol++;

                    currentCol++;
                    foreach (var activity in semesterDiscipline.Activities)
                    {
                        ActivityProtection activityProtection = student.ActivityProtections.FirstOrDefault(x => x.Activity == activity);
                        if (activityProtection != null)
                        {
                            worksheet.Cell(currentRow, currentCol).Value = activityProtection.Points;
                        }
                        currentCol++;
                    }

                    currentRow++;
                }

                worksheet.Row(currentRow).Style.Fill.BackgroundColor = XLColor.FromArgb(51, 51, 51); currentRow++;
                currentRow++;

                worksheet.Cell(currentRow, 2).Value = "Преподаватель";
                worksheet.Cell(currentRow, 3).Value = semesterDiscipline.Discipline.TeacherDisciplines.FirstOrDefault(x => x.DisciplineId == semesterDiscipline.DisciplineId).Teacher.FullName;
                worksheet.Range(worksheet.Cell(currentRow, 3), worksheet.Cell(currentRow, 3 + 6)).Merge();
                worksheet.Range(worksheet.Cell(currentRow, 3), worksheet.Cell(currentRow, 3 + 6)).Style.Fill.BackgroundColor = XLColor.FromArgb(254, 0, 254);

                worksheet.Column(1).AdjustToContents();
                worksheet.Column(2).AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", group.Name + ".xlsx");
                }
            }
        }

        public string Abbreviaturization(string sentence)
        {
            string[] subs = Regex.Replace(sentence, @"\s+", " ").Split(' ');
            var builder = new StringBuilder();
            foreach(var word in subs)
            {
                if (word[0] != ' ')
                {
                    builder.Append(char.ToUpper(word[0]));
                }
            }
            return builder.ToString();
        }

        public string GetDisplayName(object obj, string propertyName)
        {
            if (obj == null) return null;
            return GetDisplayName(obj.GetType(), propertyName);

        }

        public string GetDisplayName(Type type, string propertyName)
        {
            var property = type.GetProperty(propertyName);
            if (property == null) return null;

            return GetDisplayName(property);
        }

        public string GetDisplayName(PropertyInfo property)
        {
            var attrName = GetAttributeDisplayName(property);
            if (!string.IsNullOrEmpty(attrName))
                return attrName;

            var metaName = GetMetaDisplayName(property);
            if (!string.IsNullOrEmpty(metaName))
                return metaName;

            return property.Name.ToString();
        }

        private string GetAttributeDisplayName(PropertyInfo property)
        {
            var atts = property.GetCustomAttributes(
                typeof(DisplayNameAttribute), true);
            if (atts.Length == 0)
                return null;
            return (atts[0] as DisplayNameAttribute).DisplayName;
        }

        private string GetMetaDisplayName(PropertyInfo property)
        {
            var atts = property.DeclaringType.GetCustomAttributes(
                typeof(MetadataTypeAttribute), true);
            if (atts.Length == 0)
                return null;

            var metaAttr = atts[0] as MetadataTypeAttribute;
            var metaProperty =
                metaAttr.MetadataClassType.GetProperty(property.Name);
            if (metaProperty == null)
                return null;
            return GetAttributeDisplayName(metaProperty);
        }

        private bool RatingControlExists(int id)
        {
            return _context.RatingControls.Any(e => e.Id == id);
        }
    }
}
