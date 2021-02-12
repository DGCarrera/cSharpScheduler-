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
using WGUClassScheduler.Resources.forms;
using Xamarin.Forms;
using Xamarin.Forms.Platform;
using static Android.App.DatePickerDialog;

namespace WGUClassScheduler
{
    [Activity(Label = "FrontPageActivity")]
    public class FrontPageActivity : Activity, IOnDateSetListener
    {
        private Android.Widget.Button _professorPage;
        private Android.Widget.Button _StartDate;
        private Android.Widget.Button _EndDate;
        private Android.Widget.Button _StartDateEdit;
        private Android.Widget.Button _EndDateEdit;
        private Android.Widget.Button _DeleteForm;
        private Android.Widget.Button _AddForm;
        private Android.Widget.Button _DeleteClass;
        private Android.Widget.Button _EditClass;
        private Android.Widget.Button _SubmitEditClass;
        private Android.Widget.Button _switchFormRecycler;
        private Android.Widget.Button _Assessmet1StartAdd, _Assessmet1EndAdd, _Assessmet2StartAdd, _Assessmet2EndAdd, _Assessmet1StartEdit, _Assessmet1EndEdit, _Assessmet2StartEdit, _Assessmet2EndEdit;
        DateTime dateTimeStartDate, dateTimeEndDate, dateTimeAssessmet2StartAdd, dateTimeAssessmet2EndAdd, dateTimeAssessmet1EndAdd, dateTimeAssessmet1StartAdd;
        DateTime dateTimeStartDateEdit, dateTimeEndDateEdit, dateTimeAssessmet2StartEdit, dateTimeAssessmet2EndEdit, dateTimeAssessmet1EndEdit, dateTimeAssessmet1StartEdit;
        //private Android.Widget.Button _Courseinstructor;
        private ClassRepository frontPageRepository = new ClassRepository();
        private ProfessorRepository professorRepository = new ProfessorRepository();
        private Android.Widget.Button _frontPageButton;
        private RecyclerView _frontPageRecyclerView;
        private RecyclerView.LayoutManager _frontPageLayoutManager;
        private FrontPageAdapter _frontPageAdapter;
        private ClassInfo tempClass = new ClassInfo();
        List<ClassInfo> ClassListTemp;
        List<Instructor> InstructorListTemp;
        List<string> ProfList;
        List<string> ClassStringList;
        List<string> AssesmentTypes;
        List<string> StatusTypes;
        private const int DATE_DIALOG = 1;
        //private int year, month, day;
        private int startclick = 0, endClick = 0, profId, endClickEdit = 0, startClickEdit = 0, Assessmet1StartAdd = 0, Assessmet1EndAdd = 0, Assessmet2StartAdd = 0, Assessmet2EndAdd = 0,
            Assessmet1StartEdit = 0, Assessmet1EndEdit = 0, Assessmet2StartEdit = 0, Assessmet2EndEdit = 0; 
        private string Editstatus, Addstatus, stringDate, stringStartDate, stringEndDate, stringStartDateEdit, stringEndDateEdit, profNameId, deleteItemNameId, Assess1, Assess2, Assess1Edit, Assess2Edit, stringClassEdit, professoredit;
        private string Assessmet1StartAddS, Assessmet1EndAddS, Assessmet2StartAddS, Assessmet2EndAddS, Assessmet1StartEditS, Assessmet1EndEditS, Assessmet2StartEditS, Assessmet2EndEditS;
        DateTime today = DateTime.Today;
        bool switchForm = true;
        //AssesmentPicker picker = new AssesmentPicker();
        //public List<string> ProfessorNames { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.front_page);
            FindView();
            LinkEventHandlers();
            var deleteView = FindViewById<LinearLayout>(Resource.Id.DeleteClassForm);
            var editView = FindViewById<LinearLayout>(Resource.Id.FrontPageEdit);
            var classForm = FindViewById<LinearLayout>(Resource.Id.classFormToggle);
            deleteView.Visibility = ViewStates.Invisible;
            editView.Visibility = ViewStates.Invisible;
            classForm.Visibility = ViewStates.Invisible;
            frontPageRepository.LoadClasses();
            ClassListTemp = frontPageRepository.GetAllClasses();
            professorRepository.LoadInstructors();
            InstructorListTemp = professorRepository.GetAllProfessors();
            _frontPageRecyclerView = FindViewById<RecyclerView>(Resource.Id.frontPageRecyclerView);
            _frontPageLayoutManager = new LinearLayoutManager(this);
            _frontPageRecyclerView.SetLayoutManager(_frontPageLayoutManager);
            _frontPageAdapter = new FrontPageAdapter();
            _frontPageAdapter.ItemClick += OnItemClick;
            _frontPageRecyclerView.SetAdapter(_frontPageAdapter);
            var spinner = FindViewById<Spinner>(Resource.Id.ProfessorSpinner);
            var deletSpinner = FindViewById<Spinner>(Resource.Id.DeletSpinner);
            var ClassSpinnerEdit = FindViewById<Spinner>(Resource.Id.ClassSpinnerEdit);
            var ProfessorSpinnerEdit = FindViewById<Spinner>(Resource.Id.ProfessorSpinnerEdit);
            ProfList = setProfSpinner();
            ClassStringList = setClassSpinner();
            var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, ProfList);
            var adapter2 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, ClassStringList);
            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            deletSpinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(deletSpinner_ItemSelected);
            ClassSpinnerEdit.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(classSpinnerEdit_ItemSelected);
            ProfessorSpinnerEdit.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(professorSpinnerEdit_ItemSelected);
            spinner.Adapter = adapter;
            deletSpinner.Adapter = adapter2;
            ClassSpinnerEdit.Adapter = adapter2;
            ProfessorSpinnerEdit.Adapter = adapter;
            var spinnerStatusAdd = FindViewById<Spinner>(Resource.Id.ActiveSpinnerAdd);
            var spinnerStatusEdit = FindViewById<Spinner>(Resource.Id.ActiveSpinnerEdit);
            var spinnerAssess1 = FindViewById<Spinner>(Resource.Id.CourseAssesment1);
            var spinnerAssess2 = FindViewById<Spinner>(Resource.Id.CourseAssesment2);
            var spinnerAssess1Edit = FindViewById<Spinner>(Resource.Id.CourseAssesment1Edit);
            var spinnerAssess2Edit = FindViewById<Spinner>(Resource.Id.CourseAssesment2Edit);
            AssesmentTypes = setAssesmentTypes();
            StatusTypes = setStatusType();
            spinnerStatusAdd.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinnerStatusAdd_ItemSelected);
            spinnerStatusEdit.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinnerStatusEdit_ItemSelected);
            spinnerAssess1.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinnerAssess1_ItemSelected);
            spinnerAssess2.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinnerAssess2_ItemSelected);
            spinnerAssess1Edit.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinnerAssess1Edit_ItemSelected);
            spinnerAssess2Edit.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinnerAssess2Edit_ItemSelected);
            var assesmentAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, AssesmentTypes);
            var StatusAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, StatusTypes);
            spinnerAssess1.Adapter = assesmentAdapter;
            spinnerAssess2.Adapter = assesmentAdapter;
            spinnerAssess1Edit.Adapter = assesmentAdapter;
            spinnerAssess2Edit.Adapter = assesmentAdapter;
            spinnerStatusAdd.Adapter = StatusAdapter;
            spinnerStatusEdit.Adapter = StatusAdapter;
            //PickerModel();
            // Create your application here
        }

        private void spinnerStatusEdit_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = sender as Spinner;
            var tempItem = spinner.GetItemAtPosition(e.Position).ToString();
            if (tempItem != "Select Status") { Editstatus = tempItem; }
        }
        private void spinnerStatusAdd_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = sender as Spinner;
            var tempItem = spinner.GetItemAtPosition(e.Position).ToString();
            if (tempItem != "Select Status") { Addstatus = tempItem; }
        }
        private void professorSpinnerEdit_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = sender as Spinner;
            var tempItem = spinner.GetItemAtPosition(e.Position).ToString();
            if (tempItem != "Select Professor") { professoredit = tempItem.Split(":")[1]; }
        }
        private DateTime DateConverter(string temp)
        {
            string[] tempArray = temp.Split("/");
            int month = int.Parse(tempArray[0]);
            int day = int.Parse(tempArray[1]);
            int year = int.Parse(tempArray[2]);
            DateTime tempDate = new DateTime(year, month, day, 10, 0, 0);
            return tempDate;
        } 
        private void classSpinnerEdit_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = sender as Spinner;
            var tempItem = spinner.GetItemAtPosition(e.Position).ToString();
            if (tempItem == "Select Class")  return;  
            stringClassEdit = tempItem.Split(":")[1];
            var tempName = FindViewById<EditText>(Resource.Id.ClassNameEdit);
            var tempStartDate = FindViewById<Android.Widget.Button>(Resource.Id.StartDateEdit);
            var tempEndDate = FindViewById<Android.Widget.Button>(Resource.Id.EndDateEdit);
            var tempCourseAssesment = FindViewById<Spinner>(Resource.Id.CourseAssesment1Edit);
            var tempCourseAssesment2 = FindViewById<Spinner>(Resource.Id.CourseAssesment2Edit);
            var tempInstructorId = FindViewById<Spinner>(Resource.Id.ProfessorSpinnerEdit);
            var tempMemo = FindViewById<EditText>(Resource.Id.MemoEdit);
            var tempId = FindViewById<TextView>(Resource.Id.ClassIdEdit);
            var tempItemId = int.Parse(tempItem.Split(":")[1]);
            var tempAssessment1Name = FindViewById<EditText>(Resource.Id.CourseAssesment1NameEdit);
            var tempAssessment1StartDate = FindViewById<Android.Widget.Button>(Resource.Id.CourseAssesment1StartDateEdit);
            var tempAssessment1EndDate = FindViewById<Android.Widget.Button>(Resource.Id.CourseAssesment1EndDateEdit);
            var tempAssessment2Name = FindViewById<EditText>(Resource.Id.CourseAssesment2NameEdit);
            var tempAssessment2StartDate = FindViewById<Android.Widget.Button>(Resource.Id.CourseAssesment2StartDateEdit);
            var tempAssessment2EndDate = FindViewById<Android.Widget.Button>(Resource.Id.CourseAssesment2EndDateEdit);
            var tempClassStatus = FindViewById<Spinner>(Resource.Id.ActiveSpinnerEdit);

           
            foreach (ClassInfo item in ClassListTemp)
            {

                if (tempItemId == item.Id )
                {
                    tempName.Text = item.Name;
                    tempStartDate.Text = item.StartDate;
                    dateTimeStartDateEdit = DateConverter(item.StartDate);
                    tempEndDate.Text = item.EndDate;
                    dateTimeEndDateEdit = DateConverter(item.EndDate);
                    if (item.AssesmentType == 0) { tempCourseAssesment.SetSelection(1); } else { tempCourseAssesment.SetSelection(2); }
                    if (item.AssesmentType2 == 0) { tempCourseAssesment2.SetSelection(1); } else { tempCourseAssesment2.SetSelection(2); }
                    for (int i = 0; i < InstructorListTemp.Count; i++)
                    {
                        if (item.Id == InstructorListTemp[i].Id)
                        {
                            tempInstructorId.SetSelection(i + 1);
                            break;
                        }
                    }
                    tempAssessment1Name.Text = item.Asssesment1Name;
                    tempAssessment2Name.Text = item.Asssesment2Name;
                    tempAssessment1StartDate.Text = item.Assessment1StartDate;
                    dateTimeAssessmet1StartEdit = DateConverter(item.Assessment1StartDate);
                    tempAssessment1EndDate.Text = item.Assessment1EndDate;
                    dateTimeAssessmet1EndEdit = DateConverter(item.Assessment1EndDate);
                    tempAssessment2StartDate.Text = item.Assessment2StartDate;
                    dateTimeAssessmet2StartEdit = DateConverter(item.Assessment2StartDate);
                    tempAssessment2EndDate.Text = item.Assessment2EndDate;
                    dateTimeAssessmet2EndEdit = DateConverter(item.Assessment2EndDate);
                    if (item.ClassStatus == "In Progress") tempClassStatus.SetSelection(1);
                    else if (item.ClassStatus == "Completed") tempClassStatus.SetSelection(2);
                    else if (item.ClassStatus == "Dropped") tempClassStatus.SetSelection(3);
                    else if (item.ClassStatus == "Waiting") tempClassStatus.SetSelection(4);
                    for (int i = 0; i < InstructorListTemp.Count; i++)
                    {
                        if (InstructorListTemp[i].Id == item.InstructorId)
                        {
                            tempInstructorId.SetSelection(i + 1);
                        }
                    }
                    tempMemo.Text = item.Memo;
                    tempId.Text = item.Id.ToString();
                    return;
                       
                }
            }

        }
        private void spinnerAssess1_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = sender as Spinner;
            var tempItem = spinner.GetItemAtPosition(e.Position).ToString();
            Assess1 = tempItem;
        }
        private void spinnerAssess2_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = sender as Spinner;
            var tempItem = spinner.GetItemAtPosition(e.Position).ToString();
            Assess2 = tempItem;
        }
        private void spinnerAssess1Edit_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = sender as Spinner;
            var tempItem = spinner.GetItemAtPosition(e.Position).ToString();
            Assess1Edit = tempItem;
        }
        private void spinnerAssess2Edit_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = sender as Spinner;
            var tempItem = spinner.GetItemAtPosition(e.Position).ToString();
            Assess2Edit = tempItem;
        }
        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = sender as Spinner;
            var tempItem = spinner.GetItemAtPosition(e.Position).ToString();
            profNameId = tempItem;
        }
        private void deletSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = sender as Spinner;
            var tempItem = spinner.GetItemAtPosition(e.Position).ToString();
            deleteItemNameId = tempItem;
        }

            private void LinkEventHandlers()
        {
            //_professorPage.Click += __professorPage_Click;
            _frontPageButton.Click += _frontPageButton_Click;
            _StartDate.Click += _StartDate_Click;
            _EndDate.Click += _EndDate_Click;
            _DeleteForm.Click += _DeleteForm_Click;
            _AddForm.Click += _AddForm_Click;
            _DeleteClass.Click += _DeleteClass_Click;
            _EditClass.Click += _EditClass_Click;
            _StartDateEdit.Click += _StartDateEdit_Click;
            _EndDateEdit.Click += _EndDateEdit_Click;
            _SubmitEditClass.Click += _SubmitEditClass_Click;
            _Assessmet1StartAdd.Click += _Assessmet1StartAdd_Click;
            _Assessmet1EndAdd.Click += _Assessmet1EndAdd_Click;
            _Assessmet2StartAdd.Click += _Assessmet2StartAdd_Click;
            _Assessmet2EndAdd.Click += _Assessmet2EndAdd_Click;
            _Assessmet1StartEdit.Click += _Assessmet1StartEdit_Click;
            _Assessmet1EndEdit.Click += _Assessmet1EndEdit_Click;
            _Assessmet2StartEdit.Click += _Assessmet2StartEdit_Click;
            _Assessmet2EndEdit.Click += _Assessmet2EndEdit_Click;
            _switchFormRecycler.Click += _switchFormRecycler_Click;
            //_Courseinstructor.Click += _Courseinstructor_Click; 
        }

        private void _switchFormRecycler_Click(object sender, EventArgs e)
        {
            var classForm = FindViewById<LinearLayout>(Resource.Id.classFormToggle);
            var classRecycler = FindViewById<Android.Widget.ScrollView>(Resource.Id.recyclerViewToggle);
            if (switchForm == true)
            {
                classForm.Visibility = ViewStates.Visible;
                classRecycler.Visibility = ViewStates.Invisible;
                switchForm = false;
            } else
            {
                classForm.Visibility = ViewStates.Invisible;
                classRecycler.Visibility = ViewStates.Visible;
                switchForm = true;
            }

        }

        private void _Assessmet2EndEdit_Click(object sender, EventArgs e)
        {
            Assessmet2EndEdit++;
            ShowDialog(DATE_DIALOG);
        }

        private void _Assessmet2StartEdit_Click(object sender, EventArgs e)
        {
            Assessmet2StartEdit++;
            ShowDialog(DATE_DIALOG);
        }

        private void _Assessmet1EndEdit_Click(object sender, EventArgs e)
        {
            Assessmet1EndEdit++;
            ShowDialog(DATE_DIALOG);
        }

        private void _Assessmet1StartEdit_Click(object sender, EventArgs e)
        {
            Assessmet1StartEdit++;
            ShowDialog(DATE_DIALOG);
        }

        private void _Assessmet2EndAdd_Click(object sender, EventArgs e)
        {
            Assessmet2EndAdd++;
            ShowDialog(DATE_DIALOG);
        }

        private void _Assessmet2StartAdd_Click(object sender, EventArgs e)
        {
            Assessmet2StartAdd++;
            ShowDialog(DATE_DIALOG);
        }

        private void _Assessmet1EndAdd_Click(object sender, EventArgs e)
        {
            Assessmet1EndAdd++;
            ShowDialog(DATE_DIALOG);
        }

        private void _Assessmet1StartAdd_Click(object sender, EventArgs e)
        {
            Assessmet1StartAdd++;
            ShowDialog(DATE_DIALOG);
        }
        
        private void _SubmitEditClass_Click(object sender, EventArgs e)
        {
            errorHandlingFunctions newerror = new errorHandlingFunctions();
            var tempName = FindViewById<EditText>(Resource.Id.ClassNameEdit);
            var tempStartDate = FindViewById<Android.Widget.Button>(Resource.Id.StartDateEdit);
            var tempEndDate = FindViewById<Android.Widget.Button>(Resource.Id.EndDateEdit);
            var tempMemo = FindViewById<EditText>(Resource.Id.MemoEdit);
            var tempId = FindViewById<TextView>(Resource.Id.ClassIdEdit);
            var tempAssessment1Name = FindViewById<EditText>(Resource.Id.CourseAssesment1NameEdit);
            var tempAssessment1StartDate = FindViewById<Android.Widget.Button>(Resource.Id.CourseAssesment1StartDateEdit);
            var tempAssessment1EndDate = FindViewById<Android.Widget.Button>(Resource.Id.CourseAssesment1EndDateEdit);
            var tempAssessment2Name = FindViewById<EditText>(Resource.Id.CourseAssesment2NameEdit);
            var tempAssessment2StartDate = FindViewById<Android.Widget.Button>(Resource.Id.CourseAssesment2StartDateEdit);
            var tempAssessment2EndDate = FindViewById<Android.Widget.Button>(Resource.Id.CourseAssesment2EndDateEdit);
            var tempClassStatus = FindViewById<Spinner>(Resource.Id.ActiveSpinnerEdit);
            var ProfessorSpinnerEdit = FindViewById<Spinner>(Resource.Id.ProfessorSpinnerEdit);
            var CourseAssesment2Edit = FindViewById<Spinner>(Resource.Id.CourseAssesment2Edit);
            var CourseAssesment1Edit = FindViewById<Spinner>(Resource.Id.CourseAssesment1Edit);
            var ClassSpinnerEdit = FindViewById<Spinner>(Resource.Id.ClassSpinnerEdit);
            var tempMessage = newerror.compareDates(dateTimeStartDateEdit, dateTimeEndDateEdit);
            var tempMessage2 = newerror.compareDates(dateTimeStartDateEdit, dateTimeEndDateEdit, dateTimeAssessmet1StartEdit, dateTimeAssessmet1EndEdit, "Class", 1);
            var tempMessage3 = newerror.compareDates(dateTimeStartDateEdit, dateTimeEndDateEdit, dateTimeAssessmet2StartEdit, dateTimeAssessmet2EndEdit, "Class", 2);
            

            foreach (ClassInfo item in ClassListTemp)
            {
              if(int.Parse(stringClassEdit) == item.Id ) {

                    if (tempMessage != "N/A") { Toast.MakeText(this, tempMessage, ToastLength.Short).Show(); return; }
                    if (tempMessage2 != "N/A") { Toast.MakeText(this, tempMessage2, ToastLength.Short).Show(); return; }
                    if (tempMessage3 != "N/A") { Toast.MakeText(this, tempMessage3, ToastLength.Short).Show(); return; }
                    if (Assess1Edit == "Performance") { item.AssesmentType = 0; } else if (Assess1Edit == "Objective") { item.AssesmentType = 1; } else { Toast.MakeText(this, "Please choose an Assesment 1 option", ToastLength.Long).Show(); return; }
                    if (Assess2Edit == "Performance") { item.AssesmentType2 = 0; } else if (Assess2Edit == "Objective") { item.AssesmentType2 = 1; } else { Toast.MakeText(this, "Please choose an Assesment 2 option", ToastLength.Long).Show(); return; }
                    item.InstructorId = int.Parse(professoredit);
                    if (tempName.Text.Trim() != "") { item.Name = tempName.Text; } else { Toast.MakeText(this, "Please add a class name", ToastLength.Short).Show(); return;  }
                    item.StartDate = tempStartDate.Text;
                    item.EndDate = tempEndDate.Text;
                    item.Memo = tempMemo.Text;
                    item.Id = int.Parse(tempId.Text);
                    if (tempAssessment1Name.Text.Trim() != "") { item.Asssesment1Name = tempAssessment1Name.Text; } else { Toast.MakeText(this, "Please add a name to assessment1", ToastLength.Short).Show(); return; }
                    item.Assessment1StartDate = tempAssessment1StartDate.Text;
                    item.Assessment1EndDate = tempAssessment1EndDate.Text;
                    if (tempAssessment2Name.Text.Trim() != "") { item.Asssesment2Name = tempAssessment2Name.Text; } else { Toast.MakeText(this, "Please add a name to assessment1", ToastLength.Short).Show(); return; }
                    item.Assessment2StartDate = tempAssessment2StartDate.Text;
                    item.Assessment2EndDate = tempAssessment2EndDate.Text;
                    if (Editstatus != "Select Status") { item.ClassStatus = Editstatus; ; } else { Toast.MakeText(this, "Please choose a class status", ToastLength.Short).Show(); return; }
                    frontPageRepository.SaveEditedClass(ClassListTemp);
                    _frontPageAdapter.editList(item);
                    _frontPageAdapter.NotifyDataSetChanged();
                    tempStartDate.Text = "Start Date";
                    tempAssessment1StartDate.Text = "Start Date";
                    tempAssessment2StartDate.Text = "Start Date";
                    tempEndDate.Text = "end Date";
                    tempAssessment1EndDate.Text = "end Date";
                    tempAssessment2EndDate.Text = "end Date";
                    tempMemo.Text = " ";
                    tempId.Text = " ";
                    tempAssessment1Name.Text = " ";
                    tempAssessment2Name.Text = " ";
                    tempName.Text = " ";
                    tempClassStatus.SetSelection(0);
                    ProfessorSpinnerEdit.SetSelection(0);
                    CourseAssesment2Edit.SetSelection(0);
                    CourseAssesment1Edit.SetSelection(0);
                    ClassSpinnerEdit.SetSelection(0);
                    int baseId = item.Id * 100;
                    CrossLocalNotifications.Current.Show("Class Edited", "The class " + item.Name + " has been edited", 101, DateTime.Now.AddSeconds(5));
                    CrossLocalNotifications.Current.Show("Class Alert", "Your class " + item.Name + " starts today", baseId, dateTimeStartDateEdit);
                    CrossLocalNotifications.Current.Show("Class Alert", "Your class " + item.Name + " ends today", baseId + 1, dateTimeEndDateEdit);
                    CrossLocalNotifications.Current.Show("Assessment Alert", "Your assessment " + item.Asssesment1Name + " for class " + item.Name + " starts today", baseId + 2, dateTimeAssessmet1StartEdit);
                    CrossLocalNotifications.Current.Show("Assessment Alert", "Your assessment " + item.Asssesment1Name + " for class " + item.Name + " is due today", baseId + 3, dateTimeAssessmet1EndEdit);
                    CrossLocalNotifications.Current.Show("Assessment Alert", "Your assessment " + item.Asssesment2Name + " for class " + item.Name + " starts today", baseId + 4, dateTimeAssessmet2StartEdit);
                    CrossLocalNotifications.Current.Show("Assessment Alert", "Your assessment " + item.Asssesment2Name + " for class " + item.Name + " is due today", baseId + 5, dateTimeAssessmet2EndEdit);
                    return;
                }
            }


        }
          
        private void _EndDateEdit_Click(object sender, EventArgs e)
        {
            endClickEdit++;
            ShowDialog(DATE_DIALOG);
        }

        private void _StartDateEdit_Click(object sender, EventArgs e)
        {
            startClickEdit++;
            ShowDialog(DATE_DIALOG);
        }

        private void _EditClass_Click(object sender, EventArgs e)
        {
            var deleteView = FindViewById<LinearLayout>(Resource.Id.DeleteClassForm);
            var mainFormView = FindViewById<LinearLayout>(Resource.Id.AddClassForm);
            var editView = FindViewById<LinearLayout>(Resource.Id.FrontPageEdit);
            mainFormView.Visibility = ViewStates.Invisible;
            deleteView.Visibility = ViewStates.Invisible;
            editView.Visibility = ViewStates.Visible;
        }

        private void _DeleteClass_Click(object sender, EventArgs e)
        {
            var tempItem = int.Parse(deleteItemNameId.Split(":")[1]);
            for (int i = 0; i < ClassListTemp.Count; i++ )
            {
                if (ClassListTemp[i].Id == tempItem)
                {
                    
                    _frontPageAdapter.RemoveFromClassList(ClassListTemp[i]);
                    ClassListTemp.Remove(ClassListTemp[i]);
                    frontPageRepository.deleteClass(ClassListTemp);
                    ClassStringList = new List<string>();
                    ClassStringList = setClassSpinner();
                    var adapter2 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, ClassStringList);
                    var deletSpinner = FindViewById<Spinner>(Resource.Id.DeletSpinner);
                    deletSpinner.Adapter = adapter2;
                    _frontPageAdapter.NotifyDataSetChanged();
                }
            }
        }

        private void _AddForm_Click(object sender, EventArgs e)
        {
            var deleteView = FindViewById<LinearLayout>(Resource.Id.DeleteClassForm);
            var mainFormView = FindViewById<LinearLayout>(Resource.Id.AddClassForm);
            var editView = FindViewById<LinearLayout>(Resource.Id.FrontPageEdit);
            mainFormView.Visibility = ViewStates.Visible;
            deleteView.Visibility = ViewStates.Invisible;
            editView.Visibility = ViewStates.Invisible;
        }

        private void _DeleteForm_Click(object sender, EventArgs e)
        {
            var deleteView = FindViewById<LinearLayout>(Resource.Id.DeleteClassForm);
            var mainFormView = FindViewById<LinearLayout>(Resource.Id.AddClassForm);
            var editView = FindViewById<LinearLayout>(Resource.Id.FrontPageEdit);
            mainFormView.Visibility = ViewStates.Invisible;
            deleteView.Visibility = ViewStates.Visible;
            editView.Visibility = ViewStates.Invisible;
        }

        //private void _Courseinstructor_Click(object sender, EventArgs e)
        //{
        //    var picker = new Picker { Title = "Select a Professor", TitleColor = Color.BlueViolet };
        //    picker.ItemsSource = ProfessorNames;
        //}

        //void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        //{
        //    var picker = (Picker)sender;
        //    int selectedIndex = picker.SelectedIndex;
        //    var tempInstructorId = FindViewById<Android.Widget.Button>(Resource.Id.Courseinstructor);

        //    if (selectedIndex != -1)
        //    {
        //        var tempInstructor = InstructorListTemp[selectedIndex];
        //        tempInstructorId.Text = tempInstructor.Name;
        //        profId = tempInstructor.Id;
        //    }
        //}

        private List<string> setProfSpinner()
        {
            var items = new List<string>();
            items.Add("Select Professor");
            foreach(Instructor item in InstructorListTemp)
            {
                items.Add(item.Name + ": " + item.Id);
            }
            return items;
            
        }

        private List<string> setClassSpinner()
        {
            var items = new List<string>();
            items.Add("Select Class");
            foreach (ClassInfo item in ClassListTemp)
            {
                items.Add(item.Name + ": " + item.Id);
            }
            return items;

        }

        private List<string> setAssesmentTypes() 
        {
            var items = new List<string>();
            items.Add("Select Assesment Type");
            items.Add("Performance");
            items.Add("Objective");
            return items;
        }

        private List<string> setStatusType()
        {
            var items = new List<string>();
            items.Add("Select Status");
            items.Add("In Progress");
            items.Add("Completed");
            items.Add("Dropped");
            items.Add("Waiting");
            return items;
        }
        private int GetInstructorId(string temp)
        {
            var tempList = temp.Split(":");
            return int.Parse(tempList[1]);
        }

        private void _EndDate_Click(object sender, EventArgs e)
        {
            endClick++;
            ShowDialog(DATE_DIALOG);
        }

        private void _StartDate_Click(object sender, EventArgs e)
        {
            startclick++;
            ShowDialog(DATE_DIALOG);
        }

        protected override Dialog OnCreateDialog(int id)
        {
            switch (id) 
            {
                case DATE_DIALOG:
                    {
                        DatePickerDialog thisDate =  new DatePickerDialog(this, this, today.Year, today.Month, today.Day);
                        
                        return thisDate;
                    }
                    break;
                default:
                    break;
            }
            return null;
        }

        private void _frontPageButton_Click(object sender, EventArgs e)
        {
            
            ClassInfo tempClass2 = new ClassInfo();
            int instructoridTemp;
            errorHandlingFunctions newerror = new errorHandlingFunctions();
            if (profNameId.Trim() != "" && profNameId.Trim() != null && profNameId.Trim() != "Select Professor") { instructoridTemp = GetInstructorId(profNameId); } else { Toast.MakeText(this, "Please add a Professor", ToastLength.Short).Show(); return; }

            var stringArray = FindViewById<Spinner>(Resource.Id.ProfessorSpinner).ToString().Split(":");
            var tempName = FindViewById<EditText>(Resource.Id.ClassNameFront);
            var tempStartDate = FindViewById<Android.Widget.Button>(Resource.Id.StartDate);
            var tempEndDate = FindViewById<Android.Widget.Button>(Resource.Id.EndDate);
            var tempCourseAssesment = FindViewById<Spinner>(Resource.Id.CourseAssesment1);
            var tempAssessment1Name = FindViewById<EditText>(Resource.Id.CourseAssesment1Name);
            var tempAssessment1StartDate = FindViewById<Android.Widget.Button>(Resource.Id.CourseAssesment1StartDate);
            var tempAssessment1EndDate = FindViewById<Android.Widget.Button>(Resource.Id.CourseAssesment1EndDate);
            var tempCourseAssesment2 = FindViewById<Spinner>(Resource.Id.CourseAssesment2);
            var tempAssessment2Name = FindViewById<EditText>(Resource.Id.CourseAssesment2Name);
            var tempAssessment2StartDate = FindViewById<Android.Widget.Button>(Resource.Id.CourseAssesment2StartDate);
            var tempAssessment2EndDate = FindViewById<Android.Widget.Button>(Resource.Id.CourseAssesment2EndDate);
            var tempClassStatus = FindViewById<Spinner>(Resource.Id.ActiveSpinnerAdd);
            var tempInstructorId = FindViewById<Spinner>(Resource.Id.ProfessorSpinner);
            var tempMemo = FindViewById<EditText>(Resource.Id.Memo);
            var tempId = FindViewById<EditText>(Resource.Id.ClassId);
            var tempMessage = newerror.compareDates(dateTimeStartDate, dateTimeEndDate);
            var tempMessage2 = newerror.compareDates(dateTimeStartDate, dateTimeEndDate, dateTimeAssessmet1StartAdd, dateTimeAssessmet1EndAdd, "Class", 1);
            var tempMessage3 = newerror.compareDates(dateTimeStartDate, dateTimeEndDate, dateTimeAssessmet2StartAdd, dateTimeAssessmet2EndAdd, "Class", 2);
            if (tempMessage != "N/A") { Toast.MakeText(this, tempMessage, ToastLength.Short).Show(); return; }
            if (tempMessage2 != "N/A") { Toast.MakeText(this, tempMessage2, ToastLength.Short).Show(); return; }
            if (tempMessage3 != "N/A") { Toast.MakeText(this, tempMessage3, ToastLength.Short).Show(); return; }
            if (tempId.Text.Trim() != "") { 
                tempClass2.Id = int.Parse(FindViewById<EditText>(Resource.Id.ClassId).Text);
                foreach (ClassInfo item in ClassListTemp)
                {
                    if (tempClass2.Id == item.Id)
                    {
                        Toast.MakeText(this, "Id already in use", ToastLength.Short).Show();
                        return;
                    }
                }
            } else { Toast.MakeText(this, "Please add a Class Id", ToastLength.Short).Show(); return; }
            if (tempName.Text.Trim() != "") { tempClass2.Name = FindViewById<EditText>(Resource.Id.ClassNameFront).Text; } else { Toast.MakeText(this, "Please add a Class Name", ToastLength.Short).Show(); return; }
            if (tempStartDate.Text.Trim() != "Start Date") { tempClass2.StartDate = stringStartDate; } else { Toast.MakeText(this, "Please add a Start Date", ToastLength.Short).Show(); return; }
            if (tempEndDate.Text.Trim() != "End Date") { tempClass2.EndDate = stringEndDate; } else { Toast.MakeText(this, "Please add an End Date", ToastLength.Short).Show(); return; }
            tempClass2.InstructorId = instructoridTemp;  
            tempClass2.Memo = FindViewById<EditText>(Resource.Id.Memo).Text;
            if(Assess1 == "Performance") { tempClass2.AssesmentType = 0; } else if (Assess1 == "Objective") { tempClass2.AssesmentType = 1; } else {  Toast.MakeText(this, "Please choose an Assesment 1 option", ToastLength.Long).Show(); return; }
            if (tempAssessment1Name.Text.Trim() != "") { tempClass2.Asssesment1Name = tempAssessment1Name.Text; } else { Toast.MakeText(this, "Please add an Assessment 1 Name", ToastLength.Short).Show(); return; }
            if (tempAssessment1StartDate.Text.Trim() != "Start Date") { tempClass2.Assessment1StartDate = tempAssessment1StartDate.Text; } else { Toast.MakeText(this, "Please add an Assessment 1 Start Date", ToastLength.Short).Show(); return; }
            if (tempAssessment1EndDate.Text.Trim() != "End Date") { tempClass2.Assessment1EndDate = tempAssessment1EndDate.Text; } else { Toast.MakeText(this, "Please add an Assessment 1 End Date", ToastLength.Short).Show(); return; }
            if (tempAssessment2Name.Text.Trim() != "") { tempClass2.Asssesment2Name = tempAssessment2Name.Text; } else { Toast.MakeText(this, "Please add an Assessment 2 Name", ToastLength.Short).Show(); return; }
            if (tempAssessment2StartDate.Text.Trim() != "Start Date") { tempClass2.Assessment2StartDate = tempAssessment2StartDate.Text; } else { Toast.MakeText(this, "Please add an Assessment 2 Start Date", ToastLength.Short).Show(); return; }
            if (tempAssessment2EndDate.Text.Trim() != "End Date") { tempClass2.Assessment2EndDate = tempAssessment2EndDate.Text; } else { Toast.MakeText(this, "Please add an Assessment 2 End Date", ToastLength.Short).Show(); return; }
            if (Assess2 == "Performance") { tempClass2.AssesmentType2 = 0; } else if (Assess2 == "Objective") { tempClass2.AssesmentType2 = 1; } else { Toast.MakeText(this, "Please choose an Assesment 2 option", ToastLength.Long).Show(); return; }
            if (Addstatus != "Select Status") { tempClass2.ClassStatus = Addstatus; } else { Toast.MakeText(this, "Please choose a class status", ToastLength.Short).Show(); return; }
            _frontPageAdapter.AddToClassList(tempClass2);
            frontPageRepository.AddClasses(tempClass2);
            //ClassListTemp.Add(tempClass2);
            _frontPageAdapter.NotifyDataSetChanged();
            tempName.Text = " ";
            tempStartDate.Text = "Start Date";
            tempEndDate.Text = "End Date";
            tempAssessment1StartDate.Text = "Start Date";
            tempAssessment1EndDate.Text = "End Date";
            tempAssessment2StartDate.Text = "Start Date";
            tempAssessment2EndDate.Text = "End Date";
            tempCourseAssesment.SetSelection(0);
            tempCourseAssesment2.SetSelection(0);
            tempInstructorId.SetSelection(0);
            tempClassStatus.SetSelection(0);
            tempMemo.Text = " ";
            tempId.Text = " ";
            tempAssessment1Name.Text = " ";
            tempAssessment2Name.Text = " ";
            ClassStringList = new List<string>();
            ClassStringList = setClassSpinner();
            var adapter2 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, ClassStringList);
            var deletSpinner = FindViewById<Spinner>(Resource.Id.DeletSpinner);
            deletSpinner.Adapter = adapter2;
            var ClassSpinnerEdit = FindViewById<Spinner>(Resource.Id.ClassSpinnerEdit);
            ClassSpinnerEdit.Adapter = adapter2;
            int baseId = tempClass2.Id * 100;
            CrossLocalNotifications.Current.Show("Added Class", "The class " + tempClass2.Name + " has been added to your list", 101, DateTime.Now.AddSeconds(5));
            CrossLocalNotifications.Current.Show("Class Alert", "Your class " + tempClass2.Name + " starts today", baseId, dateTimeStartDate);
            CrossLocalNotifications.Current.Show("Class Alert", "Your class " + tempClass2.Name + " ends today", baseId + 1, dateTimeEndDate);
            CrossLocalNotifications.Current.Show("Assessment Alert", "Your assessment " + tempClass2.Asssesment1Name + " for class " + tempClass2.Name + " starts today", baseId + 2, dateTimeAssessmet1StartAdd);
            CrossLocalNotifications.Current.Show("Assessment Alert", "Your assessment " + tempClass2.Asssesment1Name + " for class " + tempClass2.Name + " is due today", baseId + 3, dateTimeAssessmet1EndAdd);
            CrossLocalNotifications.Current.Show("Assessment Alert", "Your assessment " + tempClass2.Asssesment2Name + " for class " + tempClass2.Name + " starts today", baseId + 4, dateTimeAssessmet2StartAdd);
            CrossLocalNotifications.Current.Show("Assessment Alert", "Your assessment " + tempClass2.Asssesment2Name + " for class " + tempClass2.Name + " is due today", baseId + 5, dateTimeAssessmet2EndAdd);
        }
        
        private void __professorPage_Click(object sender, EventArgs e)
        {
            Intent intentProffessor = new Intent(this, typeof(ProfessorPageActivity));
            System.Diagnostics.Debug.WriteLine("Professor Page button pressed.");
            StartActivity(intentProffessor);
        }

        void OnItemClick (object sender, int position)
        {
            int i = 0;
            Intent intentDisplayClass = new Intent(this, typeof(DisplayFormActivity));
            ClassInfo classObject = ClassListTemp[position];
            Instructor profObj;
            string tempInstructorString = "";
            foreach (Instructor item in InstructorListTemp) {
                if (classObject.InstructorId == InstructorListTemp[i].Id)
                {
                    profObj = InstructorListTemp[i];
                    tempInstructorString = professorRepository.StringifyInstructorObject(profObj);
                    break;
                }
                else
                {
                    i++;
                }
            }
            
            string tempString = frontPageRepository.StringifyClassObject(classObject);
            intentDisplayClass.PutExtra("classObject", tempString);
            intentDisplayClass.PutExtra("instructorObject", tempInstructorString);
            StartActivity(intentDisplayClass);
        }

        private void FindView()
        {
            //_professorPage = FindViewById<Android.Widget.Button>(Resource.Id.professorPage);
            _frontPageButton = FindViewById<Android.Widget.Button>(Resource.Id.FrontPageButton);
            _StartDate = FindViewById<Android.Widget.Button>(Resource.Id.StartDate);
            _EndDate = FindViewById<Android.Widget.Button>(Resource.Id.EndDate);
            _DeleteForm = FindViewById<Android.Widget.Button>(Resource.Id.DeleteFormButton);
            _AddForm = FindViewById<Android.Widget.Button>(Resource.Id.AddFormButton);
            _DeleteClass = FindViewById<Android.Widget.Button>(Resource.Id.DeleteClass);
            _EditClass = FindViewById<Android.Widget.Button>(Resource.Id.EditFormButton);
            _StartDateEdit = FindViewById<Android.Widget.Button>(Resource.Id.StartDateEdit);
            _EndDateEdit = FindViewById<Android.Widget.Button>(Resource.Id.EndDateEdit);
            _SubmitEditClass = FindViewById<Android.Widget.Button>(Resource.Id.FrontPageEditButton);
            _Assessmet1StartAdd = FindViewById<Android.Widget.Button> (Resource.Id.CourseAssesment1StartDate);
            _Assessmet1EndAdd = FindViewById<Android.Widget.Button>(Resource.Id.CourseAssesment1EndDate);
            _Assessmet2StartAdd = FindViewById<Android.Widget.Button>(Resource.Id.CourseAssesment2StartDate);
            _Assessmet2EndAdd = FindViewById<Android.Widget.Button>(Resource.Id.CourseAssesment2EndDate);
            _Assessmet1StartEdit = FindViewById<Android.Widget.Button>(Resource.Id.CourseAssesment1StartDateEdit); 
             _Assessmet1EndEdit = FindViewById<Android.Widget.Button>(Resource.Id.CourseAssesment1EndDateEdit);
            _Assessmet2StartEdit = FindViewById<Android.Widget.Button>(Resource.Id.CourseAssesment2StartDateEdit);
            _Assessmet2EndEdit = FindViewById<Android.Widget.Button>(Resource.Id.CourseAssesment2EndDateEdit);
            _switchFormRecycler = FindViewById<Android.Widget.Button>(Resource.Id.switchFormRecycler);
            //_Courseinstructor = FindViewById<Android.Widget.Button>(Resource.Id.Courseinstructor);

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
            string stringDate = DateToString(year, month, dayOfMonth);
            errorHandlingFunctions newerror = new errorHandlingFunctions();
            var tempStartDate = FindViewById<Android.Widget.Button>(Resource.Id.StartDate);
            var tempEndDate = FindViewById<Android.Widget.Button>(Resource.Id.EndDate);
            var tempEndDateEdit = FindViewById<Android.Widget.Button>(Resource.Id.EndDateEdit);
            var tempStartDateEdit = FindViewById<Android.Widget.Button>(Resource.Id.StartDateEdit);
            var CourseAssesment2StartDateEdit = FindViewById<Android.Widget.Button>(Resource.Id.CourseAssesment2StartDateEdit);
            var CourseAssesment2EndDateEdit = FindViewById<Android.Widget.Button>(Resource.Id.CourseAssesment2EndDateEdit);
            var CourseAssesment1EndDateEdit = FindViewById<Android.Widget.Button>(Resource.Id.CourseAssesment1EndDateEdit);
            var CourseAssesment1StartDateEdit = FindViewById<Android.Widget.Button>(Resource.Id.CourseAssesment1StartDateEdit);
            var CourseAssesment1StartDate = FindViewById<Android.Widget.Button>(Resource.Id.CourseAssesment1StartDate);
            var CourseAssesment1EndDate = FindViewById<Android.Widget.Button>(Resource.Id.CourseAssesment1EndDate);
            var CourseAssesment2StartDate = FindViewById<Android.Widget.Button>(Resource.Id.CourseAssesment2StartDate);
            var CourseAssesment2EndDate = FindViewById<Android.Widget.Button>(Resource.Id.CourseAssesment2EndDate);
            
            if (startclick == 1)
            {
               
                tempStartDate.Text = stringDate;
                stringStartDate = stringDate;
                dateTimeStartDate = tempDate;
                var tempMessage = newerror.compareDates(dateTimeStartDate, dateTimeEndDate);
                if (tempMessage != "N/A") { Toast.MakeText(this, tempMessage, ToastLength.Short).Show(); }
               
            }
            else if (endClick == 1)
            {
                tempEndDate.Text = stringDate;
                stringEndDate = stringDate;
                dateTimeEndDate = tempDate;
                var tempMessage = newerror.compareDates(dateTimeStartDate, dateTimeEndDate);
                if (tempMessage != "N/A") { Toast.MakeText(this, tempMessage, ToastLength.Short).Show(); }
            }
            else if (endClickEdit == 1)
            {
                tempEndDateEdit.Text = stringDate;
                stringEndDateEdit = stringDate;
                dateTimeEndDateEdit = tempDate;
                var tempMessage = newerror.compareDates(dateTimeStartDateEdit, dateTimeEndDateEdit);
                if (tempMessage != "N/A") { Toast.MakeText(this, tempMessage, ToastLength.Short).Show(); }
            }
            else if (startClickEdit == 1)
            {
                tempStartDateEdit.Text = stringDate;
                stringStartDateEdit = stringDate;
                dateTimeStartDateEdit = tempDate;
                var tempMessage = newerror.compareDates(dateTimeStartDateEdit, dateTimeEndDateEdit);
                if (tempMessage != "N/A") { Toast.MakeText(this, tempMessage, ToastLength.Short).Show(); }
            } 
            else if (Assessmet2StartEdit == 1) 
            {
                CourseAssesment2StartDateEdit.Text = stringDate;
                Assessmet2StartEditS = stringDate;
                dateTimeAssessmet2StartEdit = tempDate;
                var tempMessage = newerror.compareDates(dateTimeStartDateEdit, dateTimeEndDateEdit, dateTimeAssessmet2StartEdit, dateTimeAssessmet2EndEdit, "Class", 2);
                if (tempMessage != "N/A") { Toast.MakeText(this, tempMessage, ToastLength.Short).Show(); }
            }
            else if (Assessmet2EndEdit == 1)
            {
                CourseAssesment2EndDateEdit.Text = stringDate;
                Assessmet2EndEditS = stringDate;
                dateTimeAssessmet2EndEdit = tempDate;
                var tempMessage = newerror.compareDates(dateTimeStartDateEdit, dateTimeEndDateEdit, dateTimeAssessmet2StartEdit, dateTimeAssessmet2EndEdit, "Class", 2);
                if (tempMessage != "N/A") { Toast.MakeText(this, tempMessage, ToastLength.Short).Show(); }
            }
            else if (Assessmet1EndEdit == 1)
            {
                CourseAssesment1EndDateEdit.Text = stringDate;
                Assessmet1EndEditS = stringDate;
                dateTimeAssessmet1EndEdit = tempDate;
                var tempMessage = newerror.compareDates(dateTimeStartDateEdit, dateTimeEndDateEdit, dateTimeAssessmet1StartEdit, dateTimeAssessmet1EndEdit, "Class", 1);
                if (tempMessage != "N/A") { Toast.MakeText(this, tempMessage, ToastLength.Short).Show(); }
            }
            else if (Assessmet1StartEdit == 1)
            {
                CourseAssesment1StartDateEdit.Text = stringDate;
                Assessmet1StartEditS = stringDate;
                dateTimeAssessmet1StartEdit = tempDate;
                var tempMessage = newerror.compareDates(dateTimeStartDateEdit, dateTimeEndDateEdit, dateTimeAssessmet1StartEdit, dateTimeAssessmet1EndEdit, "Class", 1);
                if (tempMessage != "N/A") { Toast.MakeText(this, tempMessage, ToastLength.Short).Show(); }
            }
            else if (Assessmet1StartAdd == 1)
            {
                CourseAssesment1StartDate.Text = stringDate;
                Assessmet1StartAddS = stringDate;
                dateTimeAssessmet1StartAdd = tempDate;
                var tempMessage = newerror.compareDates(dateTimeStartDate, dateTimeEndDate, dateTimeAssessmet1StartAdd, dateTimeAssessmet1EndAdd, "Class", 1);
                if (tempMessage != "N/A") { Toast.MakeText(this, tempMessage, ToastLength.Short).Show();  }
            }
            else if (Assessmet1EndAdd  == 1)
            {
                CourseAssesment1EndDate.Text = stringDate;
                Assessmet1EndAddS = stringDate;
                dateTimeAssessmet1EndAdd = tempDate;
                var tempMessage = newerror.compareDates(dateTimeStartDate, dateTimeEndDate, dateTimeAssessmet1StartAdd, dateTimeAssessmet1EndAdd, "Class", 1);
                if (tempMessage != "N/A") { Toast.MakeText(this, tempMessage, ToastLength.Short).Show();  }
            }
            else if (Assessmet2StartAdd == 1)
            {
                CourseAssesment2StartDate.Text = stringDate;
                Assessmet2StartAddS = stringDate;
                dateTimeAssessmet2StartAdd = tempDate;
                var tempMessage = newerror.compareDates(dateTimeStartDate, dateTimeEndDate, dateTimeAssessmet2StartAdd, dateTimeAssessmet2EndAdd, "Class", 2);
                if (tempMessage != "N/A") { Toast.MakeText(this, tempMessage, ToastLength.Short).Show();  }
            }
            else if (Assessmet2EndAdd == 1)
            {
                CourseAssesment2EndDate.Text = stringDate;
                Assessmet2EndAddS = stringDate;
                dateTimeAssessmet2EndAdd = tempDate;
                var tempMessage = newerror.compareDates(dateTimeStartDate, dateTimeEndDate, dateTimeAssessmet2StartAdd, dateTimeAssessmet2EndAdd, "Class", 2);
                if (tempMessage != "N/A") { Toast.MakeText(this, tempMessage, ToastLength.Short).Show();  }
            }
            
            endClick = 0;
            startclick = 0;
            endClickEdit = 0;
            startClickEdit = 0;
            Assessmet2StartEdit = 0;
            Assessmet2EndEdit = 0;
            Assessmet1EndEdit = 0;
            Assessmet2EndAdd = 0;
            Assessmet1StartEdit = 0;
            Assessmet1StartAdd = 0;
            Assessmet1EndAdd = 0;
            Assessmet2StartAdd = 0;
            Assessmet2EndAdd = 0;

            
        }

        
        //public void PickerModel()
        //{
        //    ProfessorNames = new List<string>();

        //    foreach (Instructor item in InstructorListTemp)
        //    {
        //        ProfessorNames.Add(item.Name);
        //    }
        //}

        //private void ClassEndAlert()
        //{
        //    string tempdate = DateToString(today.Year, today.Month, today.Day);
        //    foreach (ClassInfo item in ClassListTemp)
        //    {
        //        if (item.EndDate == tempdate)
        //        {
        //            string thisClassName = item.Name;
        //            AlertList.Add(thisClassName);
        //        }
        //    }   
        //}

        
    }
    //public class MyPicker : Picker
    //{ 
    
    //}
}