using DataLayer.DatabaseEntities;

namespace Service.ExtensionMethods
{
    public static class AssignmentHistoryExtensions
    {
        public static AssignmentHistory GetCurrentAssignedHistory(this List<AssignmentHistory> assignmentHistories)
        {
            return assignmentHistories.FirstOrDefault(a => a.IsCurrent);
        }
    }
}
