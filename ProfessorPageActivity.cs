using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using ClassLibraryCore.Model;
using ClassLibraryCore.Repository;
using Plugin.LocalNotifications;
using WGUClassScheduler.Adapters;

namespace WGUClassScheduler
{
    [Activity(Label = "ProfessorPageActivity")]
    public class ProfessorPageActivity : Activity
    {
        private Button _addProfessorButton;
        private Button _addProfessorFormButton;
        private Button _deleteProfessorFormButton;
        private Button _professorEditForm;
        private Button _professorDeletionButton;
        private Button _editProfessorButton;
        private RecyclerView _professorRecyclerView;
        private RecyclerView.LayoutManager _professorLayoutManager;
        private ProfessorAdapter _professorAdapter;
        private List<Instructor> thisProfList = new List<Instructor>();
        private List<string> thisProfString = new List<string>();
        private ProfessorRepository thisRepository = new ProfessorRepository();
        private string profDelete;
        int profId;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.professor_page);
            _professorRecyclerView = FindViewById<RecyclerView>(Resource.Id.professorPageRecyclerView);
            // Create your application here
            _professorLayoutManager = new LinearLayoutManager(this);
            _addProfessorButton = FindViewById<Button>(Resource.Id.addProfessorButton);
            _professorRecyclerView.SetLayoutManager(_professorLayoutManager);
            _professorAdapter = new ProfessorAdapter();
            _professorRecyclerView.SetAdapter(_professorAdapter);
            thisRepository.LoadInstructors();
            thisProfList = thisRepository.GetAllProfessors();
            thisProfString = setProfSpinner();
            FindView();
            LinkEventHandlers();
            var spinnerProfDelete = FindViewById<Spinner>(Resource.Id.ProfessorDeleteSpinner);
            spinnerProfDelete.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinnerProfDelete_ItemSelected);
            var profSpinnerEdit = FindViewById<Spinner>(Resource.Id.professorIdEdit);
            profSpinnerEdit.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinnerProfEdit_ItemSelected);
            var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, thisProfString);
            spinnerProfDelete.Adapter = adapter;
            profSpinnerEdit.Adapter = adapter;
            var deleteformprof = FindViewById<LinearLayout>(Resource.Id.ProfessorDeleteForm);
            var editFormProf = FindViewById<LinearLayout>(Resource.Id.ProfessorEditForm);
            deleteformprof.Visibility = ViewStates.Invisible;
            editFormProf.Visibility = ViewStates.Invisible;
            
        }
       
        private void spinnerProfDelete_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = sender as Spinner;
            string tempItem = spinner.GetItemAtPosition(e.Position).ToString();
            profDelete = tempItem;
        }
        private void spinnerProfEdit_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
           
            var spinner = sender as Spinner;
            string tempItem = spinner.GetItemAtPosition(e.Position).ToString();
            if (tempItem != "Select Professor") { profId = int.Parse(tempItem.Split(":")[1]); } else { return; } 
            foreach (Instructor item in thisProfList)
            {
                if (profId == item.Id )
                {
                    var NameEdit = FindViewById<EditText>(Resource.Id.professorsNameEdit);
                    var PhoneEdit = FindViewById<EditText>(Resource.Id.professorsPhoneEdit);
                    var EmailEdit = FindViewById<EditText>(Resource.Id.professorEmailEdit);
                    NameEdit.Text = item.Name;
                    PhoneEdit.Text = item.PhoneNumber;
                    EmailEdit.Text = item.Email;
                    return;
                }
            }
        }

        private void FindView()
        {
            _addProfessorFormButton = FindViewById<Android.Widget.Button> (Resource.Id.ProfessorOpenAddForm);
            _deleteProfessorFormButton = FindViewById<Android.Widget.Button>(Resource.Id.ProfessorOpenDeleteForm);
            _professorDeletionButton = FindViewById<Android.Widget.Button>(Resource.Id.ProfessorDeletionButton);
            _professorEditForm = FindViewById<Android.Widget.Button>(Resource.Id.ProfessorOpenEditForm);
            _editProfessorButton = FindViewById<Android.Widget.Button>(Resource.Id.editProfessorButton);
        }
            private void LinkEventHandlers()
        {
            _addProfessorButton.Click += _addProfessorButton_Click;
            _addProfessorFormButton.Click += _addProfessorFormButton_Click;
            _deleteProfessorFormButton.Click += _deleteProfessorFormButton_Click;
            _professorDeletionButton.Click += _professorDeletionButton_Click;
            _professorEditForm.Click += _professorEditForm_Click;
            _editProfessorButton.Click += _editProfessorButton_Click;
        }

        private void _editProfessorButton_Click(object sender, EventArgs e)
        {
            errorHandlingFunctions newerror = new errorHandlingFunctions();
            var NameEdit = FindViewById<EditText>(Resource.Id.professorsNameEdit);
            var PhoneEdit = FindViewById<EditText>(Resource.Id.professorsPhoneEdit);
            var EmailEdit = FindViewById<EditText>(Resource.Id.professorEmailEdit);

            
                foreach(Instructor item in thisProfList)
                {
                    if(item.Id == profId)
                    {
                        if (NameEdit.Text.Trim() != "") { item.Name = NameEdit.Text; } else { Toast.MakeText(this, "Please add a valid name", ToastLength.Short).Show(); return; }
                        if (PhoneEdit.Text.Length == 10) { item.PhoneNumber = PhoneEdit.Text; } else { Toast.MakeText(this, "Please add a 10 digit number, including area code. No spaces or symbols", ToastLength.Short).Show(); return; }
                        if (newerror.validEmail(EmailEdit.Text) == true) { item.Email = EmailEdit.Text; } else {Toast.MakeText(this, "Please add a valid email", ToastLength.Short).Show(); return; }
                        
                        _professorAdapter.editList(item);
                        thisRepository.SaveEditedProf(thisProfList);
                        _professorAdapter.NotifyDataSetChanged();
                    }
                }
        }
        
        private void _professorEditForm_Click(object sender, EventArgs e)
        {
            var editFormProf = FindViewById<LinearLayout>(Resource.Id.ProfessorEditForm);
            var deleteView = FindViewById<LinearLayout>(Resource.Id.ProfessorDeleteForm);
            var addView = FindViewById<LinearLayout>(Resource.Id.ProfessorAddForm);
            deleteView.Visibility = ViewStates.Invisible;
            addView.Visibility = ViewStates.Invisible;
            editFormProf.Visibility = ViewStates.Visible;
        }

        private void _professorDeletionButton_Click(object sender, EventArgs e)
        {
            var tempItem = int.Parse(profDelete.Split(":")[1]);
            for (int i = 0; i < thisProfList.Count; i++)
            {
                if (thisProfList[i].Id == tempItem)
                {

                    _professorAdapter.RemoveFromProfList(thisProfList[i]);
                    thisProfList.Remove(thisProfList[i]);
                    thisRepository.DeleteProfessor(thisProfList);
                    thisProfString = new List<string>();
                    thisProfString = setProfSpinner();
                    var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, thisProfString);
                    var spinnerProfDelete = FindViewById<Spinner>(Resource.Id.ProfessorDeleteSpinner);
                    var profSpinnerEdit = FindViewById<Spinner>(Resource.Id.professorIdEdit);
                    spinnerProfDelete.Adapter = adapter;
                    profSpinnerEdit.Adapter = adapter;
                    _professorAdapter.NotifyDataSetChanged();
                    return;
                }
            }
        }

        private void _deleteProfessorFormButton_Click(object sender, EventArgs e)
        {
            var editFormProf = FindViewById<LinearLayout>(Resource.Id.ProfessorEditForm);
            var deleteView = FindViewById<LinearLayout>(Resource.Id.ProfessorDeleteForm);
            var addView = FindViewById<LinearLayout>(Resource.Id.ProfessorAddForm);
            deleteView.Visibility = ViewStates.Visible;
            addView.Visibility = ViewStates.Invisible;
            editFormProf.Visibility = ViewStates.Invisible;
        }

        private void _addProfessorFormButton_Click(object sender, EventArgs e)
        {
            var editFormProf = FindViewById<LinearLayout>(Resource.Id.ProfessorEditForm);
            var deleteView = FindViewById<LinearLayout>(Resource.Id.ProfessorDeleteForm);
            var addView = FindViewById<LinearLayout>(Resource.Id.ProfessorAddForm);
            deleteView.Visibility = ViewStates.Invisible;
            addView.Visibility = ViewStates.Visible;
            editFormProf.Visibility = ViewStates.Invisible;
        }

        private void _addProfessorButton_Click(object sender, EventArgs e)
        {
            errorHandlingFunctions newerror = new errorHandlingFunctions();
            var profName = FindViewById<EditText>(Resource.Id.professorsNameAdd);
            var profPhone = FindViewById<EditText>(Resource.Id.professorsPhoneAdd);
            var profEmail = FindViewById<EditText>(Resource.Id.professorEmailAdd);
            var profId = FindViewById<EditText>(Resource.Id.professorId);
            int profIdInt;
            ProfessorRepository newRepository = new ProfessorRepository();
            Instructor temp = new Instructor();
            if (profId.Text.Trim() == "" ) { Toast.MakeText(this, "Please add an Id", ToastLength.Short).Show(); return;  } else { profIdInt = int.Parse(profId.Text); }
            //temp.Id = int.Parse(FindViewById<EditText>(Resource.Id.professorId).Text);
            foreach(Instructor item in thisProfList) 
            {
                if (profIdInt == item.Id)
                {
                    Toast.MakeText(this, "Id already in use", ToastLength.Short).Show();
                    return;
                } 
            }
            temp.Id = profIdInt;
            if (profName.Text.Trim() != "") { temp.Name = profName.Text; } else { Toast.MakeText(this, "Please add a valid name", ToastLength.Short).Show(); return; }
            if (profPhone.Text.Length == 10) { temp.PhoneNumber = profPhone.Text; } else { Toast.MakeText(this, "Please add a 10 digit number, including area code. No spaces or symbols", ToastLength.Short).Show(); return; }
            if (newerror.validEmail(profEmail.Text) == true) { temp.Email = profEmail.Text; } else { Toast.MakeText(this, "Please add a valid email", ToastLength.Short).Show(); return; }
            newRepository.AddProfessor(temp);
            _professorAdapter.AddToProfessors(temp);
            _professorAdapter.NotifyDataSetChanged();
            thisProfList.Add(temp);
            thisProfString = new List<string>();
            thisProfString = setProfSpinner();
            var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, thisProfString);
            var spinnerProfDelete = FindViewById<Spinner>(Resource.Id.ProfessorDeleteSpinner);
            var profSpinnerEdit = FindViewById<Spinner>(Resource.Id.professorIdEdit);
            spinnerProfDelete.Adapter = adapter;
            profSpinnerEdit.Adapter = adapter;
            var ProfName = FindViewById<EditText>(Resource.Id.professorsNameAdd);
            var ProfPhoneNumber = FindViewById<EditText>(Resource.Id.professorsPhoneAdd);
            var ProfEmail = FindViewById<EditText>(Resource.Id.professorEmailAdd);
            var ProfId = FindViewById<EditText>(Resource.Id.professorId);
            ProfName.Text = "";
            ProfPhoneNumber.Text = "";
            ProfEmail.Text = "";
            ProfId.Text = "";
        }
        private List<string> setProfSpinner()
        {
            var items = new List<string>();
            items.Add("Select Professor");
            foreach (Instructor item in thisProfList)
            {
                items.Add(item.Name + ": " + item.Id);
            }
            return items;

        }
    }
}