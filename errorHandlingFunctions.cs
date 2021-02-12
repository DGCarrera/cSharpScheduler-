using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace WGUClassScheduler
{
    public class errorHandlingFunctions
    {

        public string compareDates(DateTime Start, DateTime End) 
        {
            if (End != null)
            {
                if (Start == null )
                {
                    return "Please set your start date before end date";
                } else if (Start > End)
                {
                    return "Please set your start date earlier than your end date.";
                } else
                {
                    return "N/A";
                }
            } else
            {
                return "N/A";
            }
        }

        public string compareDates(DateTime Start, DateTime End, DateTime AssesStart, DateTime AssessEnd, string classTerm, int AssessNum)
        {
            if (AssesStart != null && AssessEnd != null)
            {
                if (Start != null && End != null)
                {
                    if (Start > AssesStart )
                    {
                        return "Please set assessment "+ AssessNum +" to a date after the start of your " + classTerm.ToLower();
                    } else if (AssesStart > AssessEnd)
                    {
                        return "Assesment " + AssessNum + "'s start date is after its end date";
                    } else if ( AssessEnd > End)
                    {
                        return "Assesment " + AssessNum + "'s end date is after the end of your " + classTerm;
                    } else
                    {
                        return "N/A";
                    }
                } 
                else 
                {
                   return "Please set " + classTerm.ToLower() + " start and end dates";
                }
            } 
            else
            {
                if (AssesStart == null)
                {
                    return "Please add a start date to assesment 1";
                }
                else
                {
                    return "N/A";
                }
            }
            return "N/A";
        }

        public bool validEmail(string emailaddress)
        {
            if (emailaddress.Trim() == "") { return false; }

            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}