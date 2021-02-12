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
    [Activity(Label = "ClassFormActivity")]
    public class ClassFormActivity : Activity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.front_page);

            // Create your application here
        }

        private void FillForm(int id)
        {
            ClassRepository tempClass = new ClassRepository();
            var tempList = tempClass.DeStringifyClass();
            foreach (ClassInfo item in tempList)
            {
                if (id == item.Id)
                {
                    var formId = FindViewById<TextView>(Resource.Id.ClassNameForm);
                    var formName = FindViewById<TextView>(Resource.Id.ProfNameForm);
                    var formEmail = FindViewById<TextView>(Resource.Id.ProfEmailForm);
                    var formPhone = FindViewById<TextView>(Resource.Id.ProfPhoneForm);
                    var formStartDate = FindViewById<TextView>(Resource.Id.StartDateForm);
                    var formEndDate = FindViewById<TextView>(Resource.Id.EndDateForm);
                    var formAssesment1 = FindViewById<TextView>(Resource.Id.Assesment1Form);
                    var formAssesment2 = FindViewById<TextView>(Resource.Id.Assesment2Form);
                    var formMemo = FindViewById<TextView>(Resource.Id.MemoForm);

                    formId.Text = item.Id.ToString();
                    formName.Text = item.Name;
                    formStartDate.Text = item.StartDate;
                    formEndDate.Text = item.EndDate;
                    formMemo.Text = item.Memo;

                    if (item.AssesmentType == 0)
                    {
                        formAssesment1.Text = "Performance";
                    }
                    else
                    {
                        formAssesment1.Text = "Objective";
                    }

                    if (item.AssesmentType2 == 0)
                    {
                        formAssesment1.Text = "Performance";
                    }
                    else
                    {
                        formAssesment1.Text = "Objective";
                    }
                }
            }
        }
    }
}