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

namespace WGUClassScheduler
{
    [Activity(Label = "ActivityNav")]
    public class ActivityNav : Activity
    {
        private Android.Widget.Button _goToTerm;
        private Android.Widget.Button _goToClass;
        private Android.Widget.Button _goToProf;
        protected override void OnCreate(Bundle savedInstanceState)
        {
           
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MainNavPage);
            FindView();
            LinkEventHandlers();
        }

        private void FindView()
        {
           _goToTerm = FindViewById<Android.Widget.Button>(Resource.Id.goToTerm);
           _goToClass = FindViewById<Android.Widget.Button>(Resource.Id.goToClass);
           _goToProf = FindViewById<Android.Widget.Button>(Resource.Id.goToProf);
        }

        private void LinkEventHandlers()
        {
            _goToTerm.Click += _goToTerm_Click;
            _goToClass.Click += _goToClass_Click;
            _goToProf.Click += _goToProf_Click; 
        }

        private void _goToProf_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(ProfessorPageActivity));
            StartActivity(intent);
        }

        private void _goToClass_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(FrontPageActivity));
            StartActivity(intent);
        }

        private void _goToTerm_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(TermActivity));
            StartActivity(intent);
        }

       
    }
}