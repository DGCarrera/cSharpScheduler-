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
    [Activity(Label = "DisplayTermActivity")]
    public class DisplayTermActivity : Activity
    {
        string tempString;
        string[] tempStrings;
        private Android.Widget.Button _Class1Button, _Class2Button, _Class3Button, _Class4Button, _Class5Button, _Class6Button;
        TermRepository _termRepository = new TermRepository();
        ClassRepository _classRepository = new ClassRepository();
        List<ClassInfo> classesList = new List<ClassInfo>();
        Term thisTerm = new Term();
        string Class1, Class2, Class3, Class4, Class5, Class6;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DisplayTerm);
            var myIntent = Intent;
            tempString = myIntent.GetStringExtra("termObject");
            tempStrings = tempString.Split("|");
            getTerm(tempStrings);
            setTerm();
            FindView();
            LinkEventHandlers();
           

            // Create your application here
        }

        private void FindView()
        {
            _Class1Button = FindViewById<Button>(Resource.Id.ClassButton1);
            _Class2Button = FindViewById<Button>(Resource.Id.ClassButton2);
            _Class3Button = FindViewById<Button>(Resource.Id.ClassButton3);
            _Class4Button = FindViewById<Button>(Resource.Id.ClassButton4);
            _Class5Button = FindViewById<Button>(Resource.Id.ClassButton5);
            _Class6Button = FindViewById<Button>(Resource.Id.ClassButton6);
        }

        private void LinkEventHandlers()
        {
            _Class1Button.Click += _Class1Button_Click;
            _Class2Button.Click += _Class2Button_Click;
            _Class3Button.Click += _Class3Button_Click;
            _Class4Button.Click += _Class4Button_Click;
            _Class5Button.Click += _Class5Button_Click;
            _Class6Button.Click += _Class6Button_Click;
        }

        private void _Class6Button_Click(object sender, EventArgs e)
        {
            Intent intentDisplayClass = new Intent(this, typeof(ClassFromTermActivity));
            intentDisplayClass.PutExtra("classProfObject", Class6);
            StartActivity(intentDisplayClass);
        }

        private void _Class5Button_Click(object sender, EventArgs e)
        {
            Intent intentDisplayClass = new Intent(this, typeof(ClassFromTermActivity));
            intentDisplayClass.PutExtra("classProfObject", Class5);
            StartActivity(intentDisplayClass);
        }

        private void _Class4Button_Click(object sender, EventArgs e)
        {
            Intent intentDisplayClass = new Intent(this, typeof(ClassFromTermActivity));
            intentDisplayClass.PutExtra("classProfObject", Class4);
            StartActivity(intentDisplayClass);
        }

        private void _Class3Button_Click(object sender, EventArgs e)
        {
            Intent intentDisplayClass = new Intent(this, typeof(ClassFromTermActivity));
            intentDisplayClass.PutExtra("classProfObject", Class3);
            StartActivity(intentDisplayClass);
        }

        private void _Class2Button_Click(object sender, EventArgs e)
        {
            Intent intentDisplayClass = new Intent(this, typeof(ClassFromTermActivity));
            intentDisplayClass.PutExtra("classProfObject", Class2);
            StartActivity(intentDisplayClass);
        }

        private void _Class1Button_Click(object sender, EventArgs e)
        {
            Intent intentDisplayClass = new Intent(this, typeof(ClassFromTermActivity));
            intentDisplayClass.PutExtra("classProfObject", Class1);
            StartActivity(intentDisplayClass);
        }

        private void getTerm(string[] tempArray)
        {
            string term = tempArray[0];
            string classString = tempArray[1];
            string[] classes = classString.Split("<dn>");
            int itemCount = 0;


            thisTerm = _termRepository.DestringifyTermObject(term);

            foreach (string item in classes)
            {
                if (item != "\n" && item != "," && item != " " && item != "")
                {
                    var temp = _classRepository.DeStringifyObject(item);
                    classesList.Add(temp);

                    switch(itemCount) {
                        case 0:
                            Class1 = temp.Id + ":" + temp.InstructorId;
                            itemCount++;
                            break;
                        case 1:
                            Class2 = temp.Id + ":" + temp.InstructorId;
                            itemCount++;
                            break;
                        case 2:
                            Class3 = temp.Id + ":" + temp.InstructorId;
                            itemCount++;
                            break;
                        case 3:
                            Class4 = temp.Id + ":" + temp.InstructorId;
                            itemCount++;
                            break;
                        case 4:
                            Class5 = temp.Id + ":" + temp.InstructorId;
                            itemCount++;
                            break;
                        case 5:
                            Class6 = temp.Id + ":" + temp.InstructorId;
                            itemCount++;
                            break;
                    }
                }
            }
        }

        public void setTerm()
        {
            
            var TermName = FindViewById<TextView>(Resource.Id.ViewTermTermName);
            var ClassName1 = FindViewById<TextView>(Resource.Id.ViewTermClassName1);
            var ClassStartDate1 = FindViewById<TextView>(Resource.Id.ViewTermClassStartDate1);
            var ClassEndDate1 = FindViewById<TextView>(Resource.Id.ViewTermClassEndDate1);
            var ClassName2 = FindViewById<TextView>(Resource.Id.ViewTermClassName2);
            var ClassStartDate2 = FindViewById<TextView>(Resource.Id.ViewTermClassStartDate2);
            var ClassEndDate2 = FindViewById<TextView>(Resource.Id.ViewTermClassEndDate2);
            var ClassName3 = FindViewById<TextView>(Resource.Id.ViewTermClassName3);
            var ClassStartDate3 = FindViewById<TextView>(Resource.Id.ViewTermClassStartDate3);
            var ClassEndDate3 = FindViewById<TextView>(Resource.Id.ViewTermClassEndDate3);
            var ClassName4 = FindViewById<TextView>(Resource.Id.ViewTermClassName4);
            var ClassStartDate4 = FindViewById<TextView>(Resource.Id.ViewTermClassStartDate4);
            var ClassEndDate4 = FindViewById<TextView>(Resource.Id.ViewTermClassEndDate4);
            var ClassName5 = FindViewById<TextView>(Resource.Id.ViewTermClassName5);
            var ClassStartDate5 = FindViewById<TextView>(Resource.Id.ViewTermClassStartDate5);
            var ClassEndDate5 = FindViewById<TextView>(Resource.Id.ViewTermClassEndDate5);
            var ClassName6 = FindViewById<TextView>(Resource.Id.ViewTermClassName6);
            var ClassStartDate6 = FindViewById<TextView>(Resource.Id.ViewTermClassStartDate6);
            var ClassEndDate6 = FindViewById<TextView>(Resource.Id.ViewTermClassEndDate6);

            if (thisTerm != null ) { TermName.Text = thisTerm.TermName; }
            if (classesList[0] != null ) { ClassName1.Text = classesList[0].Name; }
            if (classesList[0] != null) { ClassStartDate1.Text = classesList[0].StartDate; }
            if (classesList[0] != null) { ClassEndDate1.Text = classesList[0].EndDate; }
            if (classesList[1] != null) { ClassName2.Text = classesList[1].Name; }
            if (classesList[1] != null) { ClassStartDate2.Text = classesList[1].StartDate; }
            if (classesList[1] != null) { ClassEndDate2.Text = classesList[1].EndDate; }
            if (classesList[2] != null) { ClassName3.Text = classesList[2].Name; }
            if (classesList[2] != null) { ClassStartDate3.Text = classesList[2].StartDate; }
            if (classesList[2] != null) { ClassEndDate3.Text = classesList[2].EndDate; }
            if (classesList[3] != null) { ClassName4.Text = classesList[3].Name; }
            if (classesList[3] != null) { ClassStartDate4.Text = classesList[3].StartDate; }
            if (classesList[3] != null) { ClassEndDate4.Text = classesList[3].EndDate; }
            if (classesList[4] != null) { ClassName5.Text = classesList[4].Name; }
            if (classesList[4] != null) { ClassStartDate5.Text = classesList[4].StartDate; }
            if (classesList[4] != null) { ClassEndDate5.Text = classesList[4].EndDate; }
            if (classesList[5] != null) { ClassName6.Text = classesList[5].Name; }
            if (classesList[5] != null) { ClassStartDate6.Text = classesList[5].StartDate; }
            if (classesList[5] != null) { ClassEndDate6.Text = classesList[5].EndDate; }

        }
    }
}