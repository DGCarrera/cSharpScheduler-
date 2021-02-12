using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ClassLibraryCore.Model;
using ClassLibraryCore.Repository;

namespace WGUClassScheduler
{
    [Activity(Label = "ClassFromTermActivity")]
    public class ClassFromTermActivity : Activity
    {
        string tempStringClass;
        string tempStringInstructor;
        List<ClassInfo> AllClasses = new List<ClassInfo>();
        List<Instructor> AllInstructors = new List<Instructor>();
        ClassInfo tempObj = new ClassInfo();
        Instructor tempInstructor = new Instructor();
        ClassRepository tempRepository = new ClassRepository();
        ProfessorRepository tempInstructorRepository = new ProfessorRepository();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DisplayClass);

            var myIntent = Intent;
            tempStringClass = myIntent.GetStringExtra("classProfObject");
            tempRepository.LoadClasses();
            tempInstructorRepository.LoadInstructors();
            AllClasses = tempRepository.GetAllClasses();
            AllInstructors = tempInstructorRepository.GetAllProfessors();
            FillForm();
            // Create your application here
        }

        private void FillForm()
        {
            string[] tempClassProf = tempStringClass.Split(":");
            foreach (ClassInfo item in AllClasses)
            {
                if (tempClassProf[0] == item.Id.ToString() )
                {
                    tempObj = item;
                    break;
                } 
            }

            foreach (Instructor item in AllInstructors)
            {
                if (tempClassProf[1] == item.Id.ToString())
                {
                    tempInstructor = item;
                }
            }

            var tempName = FindViewById<TextView>(Resource.Id.ClassNameForm);
            var tempStartDate = FindViewById<TextView>(Resource.Id.StartDateForm);
            var tempEndDate = FindViewById<TextView>(Resource.Id.EndDateForm);
            var tempCourseAssesment = FindViewById<TextView>(Resource.Id.Assesment1Form);
            var tempCourseAssesment2 = FindViewById<TextView>(Resource.Id.Assesment2Form);
            var tempMemo = FindViewById<TextView>(Resource.Id.MemoForm);
            var tempProfessorName = FindViewById<TextView>(Resource.Id.ProfNameForm);
            var tempProfessorEmail = FindViewById<TextView>(Resource.Id.ProfEmailForm);
            var tempProfessorPhone = FindViewById<TextView>(Resource.Id.ProfPhoneForm);
            var DisplayAssessment1Name = FindViewById<TextView>(Resource.Id.DisplayAssessment1Name);
            var Assesment1FormStart = FindViewById<TextView>(Resource.Id.Assesment1FormStart);
            var Assesment1FormEnd = FindViewById<TextView>(Resource.Id.Assesment1FormEnd);
            var Assesment2FormName = FindViewById<TextView>(Resource.Id.Assesment2FormName);
            var Assesment2FormStart = FindViewById<TextView>(Resource.Id.Assesment2FormStart);
            var Assesment2FormEnd = FindViewById<TextView>(Resource.Id.Assesment2FormEnd);
            var DisplayClassStatus = FindViewById<TextView>(Resource.Id.DisplayClassStatus);
            tempName.Text = tempObj.Name.ToString();
            tempStartDate.Text = tempObj.StartDate.ToString();
            tempEndDate.Text = tempObj.EndDate.ToString();
            if (tempObj.AssesmentType == 0) { tempCourseAssesment.Text = "Performance"; } else { tempCourseAssesment.Text = "Objective"; }
            if (tempObj.AssesmentType2 == 0) { tempCourseAssesment2.Text = "Performance"; } else { tempCourseAssesment2.Text = "Objective"; }
            tempMemo.Text = tempObj.Memo = tempObj.Memo.ToString();
            tempProfessorName.Text = tempInstructor.Name;
            tempProfessorEmail.Text = tempInstructor.Email;
            tempProfessorPhone.Text = tempInstructor.PhoneNumber;
            DisplayAssessment1Name.Text = tempObj.Asssesment1Name;
            Assesment1FormStart.Text = tempObj.Assessment1StartDate;
            Assesment1FormEnd.Text = tempObj.Assessment1EndDate;
            Assesment2FormName.Text = tempObj.Asssesment2Name;
            Assesment2FormStart.Text = tempObj.Assessment2StartDate;
            Assesment2FormEnd.Text = tempObj.Assessment2EndDate;
            DisplayClassStatus.Text = tempObj.ClassStatus;
        }
    }
}