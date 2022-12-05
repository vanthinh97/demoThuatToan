namespace demoDictionary
{
    public class keyword
    {
        private static readonly IDictionary<string, string> _dict = new Dictionary<string, string>
        {
            { "LOG_AddUserToCourseSuccessAction", "EVT_Course_Enrollment" },
            { "LOG_UpdateCourseSuccessAction", "EVT_Updated_Course" },
            { "LOG_CompleteCourseAction", "EVT_Completed_Course" },
            { "LOG_GradedEssayAction", "EVT_Graded_Essay" },
            { "LOG_DeleteUserToCourseAction", "EVT_Course_Removed" },

            { "LOG_AddUserToTestExamSuccessAction", "EVT_Exam_Test_Enrollment" },
            { "LOG_CompleteTestExamAction", "EVT_Completed_Test_Exam" },
            { "LOG_GradedEssayExamAction", "EVT_Graded_Essay_Exam" },
            { "LOG_GradedEssayExam", "EVT_Graded_Essay_Exam" }, //sẽ xóa sau khi Question cập nhật key ngôn ngữ lên bản 63

            { "LOG_AddUserToTrainingRouteSuccessAction", "EVT_Training_Route_Enrollment" },
            { "LOG_CompleteTrainingRouteAction", "EVT_Completed_Training_Route" },

            { "LOG_UserEnrollCourseAction", "EVT_User_Enroll_Course" },
            { "LOG_AddDiscussSuccessAction", "EVT_Add_Discuss_Success" },
            { "LOG_ActivatedAccountAction", "EVT_Activated_Account" },
            { "LOG_ActiveAccountRequestAction", "EVT_Active_Account_Request" },

            { "LOG_UserEnrollmentAction", "EVT_User_Enrollment" },
            { "LOG_CourseAboutExpireAction", "EVT_Course_About_Expire" },
            { "LOG_JoiningCourseReminderAction", "EVT_Joining_Course_Reminder" },
            { "LOG_ChangePasswordAction", "EVT_Change_Password" },
            { "LOG_JoiningExamTestReminderAction", "EVT_Joining_Exam_Test_Reminder" },
            { "bổ sung sau", "EVT_User_Is_Deleted" },
            { "bổ sung moi", null },
        };

        public string MappingLanguageKey(string key)
        {
            return _dict.ContainsKey(key) ? _dict[key] : key;
        }
    }
}
