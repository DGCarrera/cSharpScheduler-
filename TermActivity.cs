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
using WGUClassScheduler.Adapters;
using static Android.App.DatePickerDialog;

namespace WGUClassScheduler
{
    [Activity(Label = "TermActivity")]
    public class TermActivity : Activity, IOnDateSetListener
    {
        List<Term> theseTerms = new List<Term>();
        List<ClassInfo> theseClasses= new List<ClassInfo>();
        List<Instructor> theseProfessors = new List<Instructor>();
        List<String> thisTermString = new List<string>();
        Term tempEditTerm = new Term();
        private RecyclerView _termRecyclerView;
        private RecyclerView.LayoutManager _termLayoutManager;
        private TermAdapter _termAdapter;
        private TermRepository thisRepository = new TermRepository();
        private ProfessorRepository ProfRepository = new ProfessorRepository();
        private ClassRepository classRepository = new ClassRepository();
        private Android.Widget.Button _SwitchDeleteButton, _SwitchEditButton, _SwitchAddButton, _addStartDate, _editStartDate, _editEndDate, _addEndDate, _AddTermToForm, _DeleteTermButton, _EditTermButton;
        List<string> ClassList = new List<string>();
        List<string> TermsList = new List<string>();
        string stringStartDate, stringEndDate, stringEndDateEdit, stringStartDateEdit, stringDate, TermEditName, Class1Add, Class2Add, Class3Add, Class4Add, Class5Add, Class6Add, TermDelete, Class1Edit, Class2Edit, Class3Edit, Class4Edit, Class5Edit, Class6Edit;
        private const int DATE_DIALOG = 1;
        private int AddTermStartClick = 0, AddTermEndClick = 0, endClickEdit = 0, startClickEdit = 0;
        DateTime today = DateTime.Today;
        DateTime AddTermStartDate, AddTermEndDate, EditTermStartDate, EditTermEndDate;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.TermMain);
            _termRecyclerView = FindViewById<RecyclerView>(Resource.Id.TermRecyclerView);
            var deleteView = FindViewById<LinearLayout>(Resource.Id.MainTermDelete);
            var editView = FindViewById<LinearLayout>(Resource.Id.MainTermEdit);
            deleteView.Visibility = ViewStates.Invisible;
            editView.Visibility = ViewStates.Invisible;
            theseTerms = thisRepository.LoadTerms();
            //theseTerms = thisRepository.GetAllTerms();
            ProfRepository.LoadInstructors();
            theseProfessors = ProfRepository.GetAllProfessors();
            classRepository.LoadClasses();
            theseClasses = classRepository.GetAllClasses();
            _termLayoutManager = new LinearLayoutManager(this);
            _termRecyclerView.SetLayoutManager(_termLayoutManager);
            _termAdapter = new TermAdapter();
            _termAdapter.ItemClick += OnItemClick;
            _termRecyclerView.SetAdapter(_termAdapter);
            setClassList();
            setTermList();
            var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, ClassList);
            var termAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, TermsList);
            var spinnerEditTermName = FindViewById<Spinner>(Resource.Id.EditTermNameSpin);  
            var spinnerDeleteTerm = FindViewById<Spinner>(Resource.Id.DeleteTermSpinner);
            var spinnerClass1 = FindViewById<Spinner>(Resource.Id.AddTermClass1Spin);
            var spinnerClass2 = FindViewById<Spinner>(Resource.Id.AddTermClass2Spin);
            var spinnerClass3 = FindViewById<Spinner>(Resource.Id.AddTermClass3Spin);
            var spinnerClass4 = FindViewById<Spinner>(Resource.Id.AddTermClass4Spin);
            var spinnerClass5 = FindViewById<Spinner>(Resource.Id.AddTermClass5Spin);
            var spinnerClass6 = FindViewById<Spinner>(Resource.Id.AddTermClass6Spin);
            var editspinnerClass1 = FindViewById<Spinner>(Resource.Id.editTermClass1Spin);
            var editspinnerClass2 = FindViewById<Spinner>(Resource.Id.editTermClass2Spin);
            var editspinnerClass3 = FindViewById<Spinner>(Resource.Id.editTermClass3Spin);
            var editspinnerClass4 = FindViewById<Spinner>(Resource.Id.editTermClass4Spin);
            var editspinnerClass5 = FindViewById<Spinner>(Resource.Id.editTermClass5Spin);
            var editspinnerClass6 = FindViewById<Spinner>(Resource.Id.editTermClass6Spin);
            spinnerEditTermName.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinnerEditTermName_ItemSelected);
            spinnerDeleteTerm.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinnerDeleteTerm_ItemSelected);
            spinnerClass1.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinnerClass1_ItemSelected);
            spinnerClass2.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinnerClass2_ItemSelected);
            spinnerClass3.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinnerClass3_ItemSelected);
            spinnerClass4.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinnerClass4_ItemSelected);
            spinnerClass5.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinnerClass5_ItemSelected);
            spinnerClass6.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinnerClass6_ItemSelected);
            editspinnerClass1.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(editspinnerClass1_ItemSelected);
            editspinnerClass2.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(editspinnerClass2_ItemSelected);
            editspinnerClass3.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(editspinnerClass3_ItemSelected);
            editspinnerClass4.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(editspinnerClass4_ItemSelected);
            editspinnerClass5.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(editspinnerClass5_ItemSelected);
            editspinnerClass6.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(editspinnerClass6_ItemSelected);
            spinnerEditTermName.Adapter = termAdapter;
            spinnerDeleteTerm.Adapter = termAdapter;
            spinnerClass1.Adapter = adapter;
            spinnerClass2.Adapter = adapter;
            spinnerClass3.Adapter = adapter;
            spinnerClass4.Adapter = adapter;
            spinnerClass5.Adapter = adapter;
            spinnerClass6.Adapter = adapter;
            editspinnerClass1.Adapter = adapter;
            editspinnerClass2.Adapter = adapter;
            editspinnerClass3.Adapter = adapter;
            editspinnerClass4.Adapter = adapter;
            editspinnerClass5.Adapter = adapter;
            editspinnerClass6.Adapter = adapter;
            LinkEventHandlers();
            FindView();
            // Create your application here
        }

        private void spinnerEditTermName_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = sender as Spinner;
            var tempItem = spinner.GetItemAtPosition(e.Position).ToString();
            if (tempItem == "Select Term") return;
            var startDate = FindViewById<Android.Widget.Button>(Resource.Id.editTermStartDate);
            var endDate = FindViewById<Android.Widget.Button>(Resource.Id.editTermEndDate);
            var thisspinnerClass1 = FindViewById<Spinner>(Resource.Id.editTermClass1Spin);
            var thisspinnerClass2 = FindViewById<Spinner>(Resource.Id.editTermClass2Spin);
            var thisspinnerClass3 = FindViewById<Spinner>(Resource.Id.editTermClass3Spin);
            var thisspinnerClass4 = FindViewById<Spinner>(Resource.Id.editTermClass4Spin);
            var thisspinnerClass5 = FindViewById<Spinner>(Resource.Id.editTermClass5Spin);
            var thisspinnerClass6 = FindViewById<Spinner>(Resource.Id.editTermClass6Spin);
           
            tempEditTerm = new Term();
            foreach (Term item in theseTerms)
            {
                if (tempItem == item.TermName)
                {
                    TermEditName = item.TermName;
                    startDate.Text = item.StartDate;
                    endDate.Text = item.EndDate;
                    string[] tempStart = startDate.Text.Split("/");
                    string[] tempEnd = endDate.Text.Split("/");
                    int startDateYear = int.Parse(tempStart[2]), startDateMonth = int.Parse(tempStart[0]), startDateDay = int.Parse(tempStart[1]);
                    int endDateYear = int.Parse(tempEnd[2]), endDateMonth = int.Parse(tempEnd[0]), endDateDay = int.Parse(tempEnd[1]);
                    EditTermStartDate = new DateTime(startDateYear, startDateMonth, startDateDay, 10, 0, 0);
                    EditTermEndDate = new DateTime(endDateYear, endDateMonth, endDateDay, 10, 0, 0);
                    for (int itemCount = 0; itemCount < theseClasses.Count; itemCount++)
                    {
                        if (theseClasses[itemCount].Id == item.Class1Id) thisspinnerClass1.SetSelection(itemCount + 1);
                        if (theseClasses[itemCount].Id == item.ClassId2) thisspinnerClass2.SetSelection(itemCount + 1);
                        if (theseClasses[itemCount].Id == item.ClassId3) thisspinnerClass3.SetSelection(itemCount + 1);
                        if (theseClasses[itemCount].Id == item.ClassId4) thisspinnerClass4.SetSelection(itemCount + 1);
                        if (theseClasses[itemCount].Id == item.ClassId5) thisspinnerClass5.SetSelection(itemCount + 1);
                        if (theseClasses[itemCount].Id == item.ClassId6) thisspinnerClass6.SetSelection(itemCount + 1);
                    }
                } 
            }


        }
        private void spinnerDeleteTerm_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = sender as Spinner;
            var tempItem = spinner.GetItemAtPosition(e.Position).ToString();
            if (tempItem != "Select Term") { TermDelete = tempItem;    }
        }
        private void editspinnerClass1_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = sender as Spinner;
            var tempItem = spinner.GetItemAtPosition(e.Position).ToString();
            if (tempItem != "Select Class") { Class1Edit = tempItem.Split(":")[1]; }
        }
        private void editspinnerClass2_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = sender as Spinner;
            var tempItem = spinner.GetItemAtPosition(e.Position).ToString();
            if (tempItem != "Select Class") { Class2Edit = tempItem.Split(":")[1]; }
        }
        private void editspinnerClass3_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = sender as Spinner;
            var tempItem = spinner.GetItemAtPosition(e.Position).ToString();
            if (tempItem != "Select Class") { Class3Edit = tempItem.Split(":")[1]; }
        }
        private void editspinnerClass4_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = sender as Spinner;
            var tempItem = spinner.GetItemAtPosition(e.Position).ToString();
            if (tempItem != "Select Class") { Class4Edit = tempItem.Split(":")[1]; }
        }
        private void editspinnerClass5_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = sender as Spinner;
            var tempItem = spinner.GetItemAtPosition(e.Position).ToString();
            if (tempItem != "Select Class") { Class5Edit = tempItem.Split(":")[1]; }
        }
        private void editspinnerClass6_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = sender as Spinner;
            var tempItem = spinner.GetItemAtPosition(e.Position).ToString();
            if (tempItem != "Select Class") { Class6Edit = tempItem.Split(":")[1]; }
        }
        private void spinnerClass1_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = sender as Spinner;
            var tempItem = spinner.GetItemAtPosition(e.Position).ToString();
            if (tempItem != "Select Class") { Class1Add = tempItem.Split(":")[1]; }
        }
        private void spinnerClass2_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = sender as Spinner;
            var tempItem = spinner.GetItemAtPosition(e.Position).ToString();
            if (tempItem != "Select Class") { Class2Add = tempItem.Split(":")[1]; }
        }
        private void spinnerClass3_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = sender as Spinner;
            var tempItem = spinner.GetItemAtPosition(e.Position).ToString();
            if (tempItem != "Select Class") { Class3Add = tempItem.Split(":")[1]; }
        }
        private void spinnerClass4_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = sender as Spinner;
            var tempItem = spinner.GetItemAtPosition(e.Position).ToString();
            if (tempItem != "Select Class") { Class4Add = tempItem.Split(":")[1]; }
        }
        private void spinnerClass5_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = sender as Spinner;
            var tempItem = spinner.GetItemAtPosition(e.Position).ToString();
            if (tempItem != "Select Class") { Class5Add = tempItem.Split(":")[1]; }
        }
        private void spinnerClass6_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = sender as Spinner;
            var tempItem = spinner.GetItemAtPosition(e.Position).ToString();
            if (tempItem != "Select Class") { Class6Add = tempItem.Split(":")[1]; }
        }

        protected override Dialog OnCreateDialog(int id)
        {
            switch (id)
            {
                case DATE_DIALOG:
                    {
                        DatePickerDialog thisDate = new DatePickerDialog(this, this, today.Year, today.Month, today.Day);

                        return thisDate;
                    }
                    break;
                default:
                    break;
            }
            return null;
        }

        void OnItemClick(object sender, int position)
        {
            int i = 0;
            //int j = 0;
            int profId = 0;
            Intent intentDisplayTerm = new Intent(this, typeof(DisplayTermActivity));
            Term termObject = theseTerms[position];
            ClassInfo tempClass = new ClassInfo();
            //Instructor profObj = new Instructor();
            
            string tempClassString = "";
            //string tempInstructorString = "";
            string finalString;
            foreach (ClassInfo item in theseClasses)
            {
                if (termObject.Class1Id == theseClasses[i].Id || termObject.ClassId2 == theseClasses[i].Id || termObject.ClassId3 == theseClasses[i].Id || termObject.ClassId4 == theseClasses[i].Id ||
                    termObject.ClassId5 == theseClasses[i].Id || termObject.ClassId6 == theseClasses[i].Id)
                {
                    tempClass = theseClasses[i];
                    profId = tempClass.InstructorId;
                    tempClassString += classRepository.StringifyClassObject(tempClass);
                    i++;
                    
                }
                else
                {
                    i++;
                }
            }

            //foreach (Instructor item in theseProfessors)
            //{
            //    if(item.Id == profId)
            //    {
            //        profObj = theseProfessors[j];
            //        tempInstructorString = ProfRepository.StringifyInstructorObject(profObj);
            //        break;
            //    } 
            //    else 
            //    {
            //        j++;
            //    }
            //}

            string tempString = thisRepository.serializeTerm(termObject);
            finalString = tempString + "|" + tempClassString;
            //+"|" + tempInstructorString;
            intentDisplayTerm.PutExtra("termObject", finalString);
            StartActivity(intentDisplayTerm);
            i = 0;
        }

        private void setClassList()
        {
            ClassList.Add("Select Class");

            foreach (ClassInfo item in theseClasses)
            {
                string tempstring = item.Name + ":" + item.Id;
                ClassList.Add(tempstring);
            }

            
        }

        private void setTermList()
        {
            TermsList.Add("Select Term");
            foreach (Term item in theseTerms)
            {
                string tempstring = item.TermName;
                TermsList.Add(tempstring);
            }
        }

        private void LinkEventHandlers()
        {
            _SwitchAddButton = FindViewById<Android.Widget.Button>(Resource.Id.AddTermButtonForm);
            _SwitchEditButton = FindViewById<Android.Widget.Button>(Resource.Id.EditTermButtonForm);
            _SwitchDeleteButton = FindViewById<Android.Widget.Button>(Resource.Id.DeleteTermButtonForm);
            _addStartDate = FindViewById<Android.Widget.Button>(Resource.Id.AddTermStartDate);
            _addEndDate = FindViewById<Android.Widget.Button>(Resource.Id.AddTermEndDate);
            _AddTermToForm = FindViewById<Android.Widget.Button>(Resource.Id.AddTermFormButton);
            _DeleteTermButton = FindViewById<Android.Widget.Button>(Resource.Id.DeleteTermButton);
            _EditTermButton = FindViewById<Android.Widget.Button>(Resource.Id.SubmitEditedForm);
            _editStartDate = FindViewById<Android.Widget.Button>(Resource.Id.editTermStartDate);
            _editEndDate = FindViewById<Android.Widget.Button>(Resource.Id.editTermEndDate);

        }

        private void FindView()
        {
            _SwitchAddButton.Click += _SwitchAddButton_Click;
            _SwitchEditButton.Click += _SwitchEditButton_Click;
            _SwitchDeleteButton.Click += _SwitchDeleteButton_Click;
            _addStartDate.Click += _addStartDate_Click;
            _addEndDate.Click += _addEndDate_Click;
            _AddTermToForm.Click += _AddTermToForm_Click;
            _DeleteTermButton.Click += _DeleteTermButton_Click;
            _EditTermButton.Click += _EditTermButton_Click;
            _editStartDate.Click += _editStartDate_Click;
            _editEndDate.Click += _editEndDate_Click;
        }

        private void _editEndDate_Click(object sender, EventArgs e)
        {
            endClickEdit++; 
            ShowDialog(DATE_DIALOG);
        }

        private void _editStartDate_Click(object sender, EventArgs e)
        {
            startClickEdit++;
            ShowDialog(DATE_DIALOG);
        }

        private void _EditTermButton_Click(object sender, EventArgs e)
        {
            Term tempTerm = new Term();
            errorHandlingFunctions newerror = new errorHandlingFunctions();
            var spinnerEditTermName = FindViewById<Spinner>(Resource.Id.EditTermNameSpin);
            var startDate = FindViewById<Android.Widget.Button>(Resource.Id.editTermStartDate);
            var endDate = FindViewById<Android.Widget.Button>(Resource.Id.editTermEndDate);
            var thisspinnerClass1 = FindViewById<Spinner>(Resource.Id.editTermClass1Spin);
            var thisspinnerClass2 = FindViewById<Spinner>(Resource.Id.editTermClass2Spin);
            var thisspinnerClass3 = FindViewById<Spinner>(Resource.Id.editTermClass3Spin);
            var thisspinnerClass4 = FindViewById<Spinner>(Resource.Id.editTermClass4Spin);
            var thisspinnerClass5 = FindViewById<Spinner>(Resource.Id.editTermClass5Spin);
            var thisspinnerClass6 = FindViewById<Spinner>(Resource.Id.editTermClass6Spin);
            if (TermEditName.Trim() != "") tempTerm.TermName = TermEditName;
            if (startDate.Text != "Start Date" && startDate.Text.Trim() != "") { tempTerm.StartDate = startDate.Text; } else { Toast.MakeText(this, "Please add term start date", ToastLength.Short).Show(); return; }
            if (startDate.Text != "End Date") { tempTerm.EndDate = endDate.Text; } else { Toast.MakeText(this, "Please add term end date", ToastLength.Short).Show(); return; }
            if (Class1Edit.Trim() != "" && Class1Edit.Trim() != "Select Class") { tempTerm.Class1Id = int.Parse(Class1Edit); } else { Toast.MakeText(this, "Please add a class to class 1", ToastLength.Short).Show(); return; }
            if (Class2Edit.Trim() != "" && Class2Edit.Trim() != "Select Class") { tempTerm.ClassId2 = int.Parse(Class2Edit); } else { Toast.MakeText(this, "Please add a class to class 2", ToastLength.Short).Show(); return; }
            if (Class3Edit.Trim() != "" && Class3Edit.Trim() != "Select Class") { tempTerm.ClassId3 = int.Parse(Class3Edit); } else { Toast.MakeText(this, "Please add a class to class 3", ToastLength.Short).Show(); return; }
            if (Class4Edit.Trim() != "" && Class4Edit.Trim() != "Select Class") { tempTerm.ClassId4 = int.Parse(Class4Edit); } else { Toast.MakeText(this, "Please add a class to class 4", ToastLength.Short).Show(); return; }
            if (Class5Edit.Trim() != "" && Class5Edit.Trim() != "Select Class") { tempTerm.ClassId5 = int.Parse(Class5Edit); } else { Toast.MakeText(this, "Please add a class to class 5", ToastLength.Short).Show(); return; }
            if (Class6Edit.Trim() != "" && Class6Edit.Trim() != "Select Class") { tempTerm.ClassId6 = int.Parse(Class6Edit); } else { Toast.MakeText(this, "Please add a class to class 6", ToastLength.Short).Show(); return; }
            var tempMessage = newerror.compareDates(EditTermStartDate, EditTermEndDate);
            if (tempMessage != "N/A") { Toast.MakeText(this, tempMessage, ToastLength.Short).Show(); return; }
            for (int i =0; i < theseTerms.Count; i++)
            {
                if (theseTerms[i].TermName == tempTerm.TermName)
                {
                    theseTerms.Remove(theseTerms[i]);
                    theseTerms.Add(tempTerm);
                    _termAdapter.editList(tempTerm);
                    thisRepository.SaveEditedTerm(theseTerms);
                    _termAdapter.NotifyDataSetChanged();
                }
            }
            startDate.Text = "Start Date";
            endDate.Text = "End Date";
            thisspinnerClass1.SetSelection(0);
            thisspinnerClass2.SetSelection(0);
            thisspinnerClass3.SetSelection(0);
            thisspinnerClass4.SetSelection(0);
            thisspinnerClass5.SetSelection(0);
            thisspinnerClass6.SetSelection(0);
            spinnerEditTermName.SetSelection(0);


        }

        private void _DeleteTermButton_Click(object sender, EventArgs e)
        {
            Term tempItem = new Term();
            int count = theseTerms.Count;
                for (int i = 0; i < count; i++ ) 
                { 
                    if (theseTerms[i].TermName == TermDelete)
                    {
                        tempItem = theseTerms[i];
                        _termAdapter.RemoveFromTermList(tempItem);
                        thisRepository.DeletTerm(tempItem);
                        theseTerms.Remove(tempItem);
                        TermsList = new List<string>();
                        setTermList();
                        var termAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, TermsList);
                        var spinnerDeleteTerm = FindViewById<Spinner>(Resource.Id.DeleteTermSpinner);
                        var spinnerEditTerm = FindViewById<Spinner>(Resource.Id.EditTermNameSpin);
                        spinnerDeleteTerm.Adapter = termAdapter;
                        spinnerEditTerm.Adapter = termAdapter;
                        _termAdapter.NotifyDataSetChanged();
                        break;
                    }
                }
        }

        private void _AddTermToForm_Click(object sender, EventArgs e)
        {
            Term tempTerm = new Term();
            errorHandlingFunctions newerror = new errorHandlingFunctions();
            var TermName = FindViewById<TextView>(Resource.Id.AddTermFormName);
            var startDate = FindViewById<Android.Widget.Button>(Resource.Id.AddTermStartDate);
            var endDate = FindViewById<Android.Widget.Button>(Resource.Id.AddTermEndDate);
            var thisspinnerClass1 = FindViewById<Spinner>(Resource.Id.AddTermClass1Spin);
            var thisspinnerClass2 = FindViewById<Spinner>(Resource.Id.AddTermClass2Spin);
            var thisspinnerClass3 = FindViewById<Spinner>(Resource.Id.AddTermClass3Spin);
            var thisspinnerClass4 = FindViewById<Spinner>(Resource.Id.AddTermClass4Spin);
            var thisspinnerClass5 = FindViewById<Spinner>(Resource.Id.AddTermClass5Spin);
            var thisspinnerClass6 = FindViewById<Spinner>(Resource.Id.AddTermClass6Spin);
            if (TermName.Text != " " && TermName.Text != null && TermName.Text != "") { tempTerm.TermName = TermName.Text; } else { Toast.MakeText(this, "Please add a Term Name", ToastLength.Short).Show(); return; }
            if (stringStartDate != " " && stringStartDate != null && stringStartDate != "") { tempTerm.StartDate = stringStartDate; } else { Toast.MakeText(this, "Please add a Start Date", ToastLength.Short).Show(); return; }
            if (stringEndDate != " " && stringEndDate != null && stringEndDate != "") { tempTerm.EndDate = stringEndDate; } else { Toast.MakeText(this, "Please add an End Date", ToastLength.Short).Show(); return; }
            if (Class1Add != " " && Class1Add != null && Class1Add != "") { tempTerm.Class1Id = int.Parse(Class1Add); } else { Toast.MakeText(this, "Please Choose a 1st Class", ToastLength.Short).Show(); return; }
            if (Class2Add != " " && Class2Add != null && Class2Add != "") { tempTerm.ClassId2 = int.Parse(Class2Add); } else { Toast.MakeText(this, "Please Choose a 2nd Class", ToastLength.Short).Show(); return; }
            if (Class3Add != " " && Class3Add != null && Class3Add != "") { tempTerm.ClassId3 = int.Parse(Class3Add); } else { Toast.MakeText(this, "Please Choose a 3rd Class", ToastLength.Short).Show(); return; }
            if (Class4Add != " " && Class4Add != null && Class4Add != "") { tempTerm.ClassId4 = int.Parse(Class4Add); } else { Toast.MakeText(this, "Please Choose a 4th Class", ToastLength.Short).Show(); return; }
            if (Class5Add != " " && Class5Add != null && Class5Add != "") { tempTerm.ClassId5 = int.Parse(Class5Add); } else { Toast.MakeText(this, "Please Choose a 5th Class", ToastLength.Short).Show(); return; }
            if (Class6Add != " " && Class6Add != null && Class6Add != "") { tempTerm.ClassId6 = int.Parse(Class6Add); } else { Toast.MakeText(this, "Please Choose a 6th Class", ToastLength.Short).Show(); return; }
            var tempMessage = newerror.compareDates(AddTermStartDate, AddTermEndDate);
            if (tempMessage != "N/A") { Toast.MakeText(this, tempMessage, ToastLength.Short).Show(); return; }
            _termAdapter.AddToTerms(tempTerm);
            thisRepository.AddTerm(tempTerm);
            //theseTerms.Add(tempTerm);
            _termAdapter.NotifyDataSetChanged();
            TermName.Text = " ";
            startDate.Text = "Start Date";
            endDate.Text = "End Date";
            thisspinnerClass1.SetSelection(0);
            thisspinnerClass2.SetSelection(0);
            thisspinnerClass3.SetSelection(0);
            thisspinnerClass4.SetSelection(0);
            thisspinnerClass5.SetSelection(0);
            thisspinnerClass6.SetSelection(0);
            TermsList = new List<string>();
            setTermList();
            var termAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, TermsList);
            var spinnerDeleteTerm = FindViewById<Spinner>(Resource.Id.DeleteTermSpinner);
            var spinnerEditTermName = FindViewById<Spinner>(Resource.Id.EditTermNameSpin);
            var spinnerEditTerm = FindViewById<Spinner>(Resource.Id.EditTermNameSpin);
            spinnerDeleteTerm.Adapter = termAdapter;
            //spinnerEditTermName.Adapter = termAdapter;
            spinnerEditTerm.Adapter = termAdapter; 

        }

        private string DateToString(int year, int month, int day)
        {
            int thisyear = year;
            int thismonth = month + 1;
            int thisday = day;
            stringDate = thismonth.ToString() + "/" + thisday.ToString() + "/" + thisyear.ToString();
            return stringDate;
        }

        public void OnDateSet(Android.Widget.DatePicker view, int year, int month, int dayOfMonth)
        {
            DateTime tempDate = new DateTime(year, month + 1, dayOfMonth, 10, 0, 0);
            errorHandlingFunctions newerror = new errorHandlingFunctions();
            string stringDate = DateToString(year, month, dayOfMonth);
            var tempStartDate = FindViewById<Android.Widget.Button>(Resource.Id.AddTermStartDate);
            var tempEndDate = FindViewById<Android.Widget.Button>(Resource.Id.AddTermEndDate);
            var tempEndDateEdit = FindViewById<Android.Widget.Button>(Resource.Id.editTermEndDate);
            var tempStartDateEdit = FindViewById<Android.Widget.Button>(Resource.Id.editTermStartDate);
            
            if (AddTermStartClick == 1)
            {
                tempStartDate.Text = stringDate;
                stringStartDate = stringDate;
                AddTermStartDate = tempDate;
                var tempMessage = newerror.compareDates(AddTermStartDate, AddTermEndDate);
                if (tempMessage != "N/A") { Toast.MakeText(this, tempMessage, ToastLength.Short).Show(); }
            }
            else if (AddTermEndClick == 1)
            {
                tempEndDate.Text = stringDate;
                stringEndDate = stringDate;
                AddTermEndDate = tempDate;
                var tempMessage = newerror.compareDates(AddTermStartDate, AddTermEndDate);
                if (tempMessage != "N/A") { Toast.MakeText(this, tempMessage, ToastLength.Short).Show(); }
            }
            else if (endClickEdit == 1)
            {
                tempEndDateEdit.Text = stringDate;
                stringEndDateEdit = stringDate;
                EditTermEndDate = tempDate;
                var tempMessage = newerror.compareDates(EditTermStartDate, EditTermEndDate);
                if (tempMessage != "N/A") { Toast.MakeText(this, tempMessage, ToastLength.Short).Show(); }
            }
            else if (startClickEdit == 1)
            {
                tempStartDateEdit.Text = stringDate;
                stringStartDateEdit = stringDate;
                EditTermStartDate = tempDate;
                var tempMessage = newerror.compareDates(EditTermStartDate, EditTermEndDate);
                if (tempMessage != "N/A") { Toast.MakeText(this, tempMessage, ToastLength.Short).Show(); }
            }
            AddTermStartClick = 0;
            AddTermEndClick = 0;
            endClickEdit = 0;
            startClickEdit = 0;
            
        }
        private void _addEndDate_Click(object sender, EventArgs e)
        {
            AddTermEndClick++;
            ShowDialog(DATE_DIALOG);
        }

        private void _addStartDate_Click(object sender, EventArgs e)
        {
            AddTermStartClick++;
            ShowDialog(DATE_DIALOG);
        }

        private void _SwitchDeleteButton_Click(object sender, EventArgs e)
        {
            var deleteView = FindViewById<LinearLayout>(Resource.Id.MainTermDelete);
            var mainFormView = FindViewById<LinearLayout>(Resource.Id.MainTermAdd);
            var editView = FindViewById<LinearLayout>(Resource.Id.MainTermEdit);
            mainFormView.Visibility = ViewStates.Invisible;
            deleteView.Visibility = ViewStates.Visible;
            editView.Visibility = ViewStates.Invisible;
        }

        private void _SwitchEditButton_Click(object sender, EventArgs e)
        {
            var deleteView = FindViewById<LinearLayout>(Resource.Id.MainTermDelete);
            var mainFormView = FindViewById<LinearLayout>(Resource.Id.MainTermAdd);
            var editView = FindViewById<LinearLayout>(Resource.Id.MainTermEdit);
            mainFormView.Visibility = ViewStates.Invisible;
            deleteView.Visibility = ViewStates.Invisible;
            editView.Visibility = ViewStates.Visible;
        }

        private void _SwitchAddButton_Click(object sender, EventArgs e)
        {
            var deleteView = FindViewById<LinearLayout>(Resource.Id.MainTermDelete);
            var mainFormView = FindViewById<LinearLayout>(Resource.Id.MainTermAdd);
            var editView = FindViewById<LinearLayout>(Resource.Id.MainTermEdit);
            mainFormView.Visibility = ViewStates.Visible;
            deleteView.Visibility = ViewStates.Invisible;
            editView.Visibility = ViewStates.Invisible;
        }
    }
}


