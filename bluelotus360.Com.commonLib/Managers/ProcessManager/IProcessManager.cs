using BL10.CleanArchitecture.Domain.Entities.ProjectManagement;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.commonLib.Managers.ProcessManager
{
    public interface IProcessManager:IManager
    {
        Task<IList<CodeBaseResponseExtended>> GetKanbanBoard(ProcessRequest responses);
        Task<IList<Process>> GetListOfTasks(ProcessRequest responses);
        Task<IList<CodeBaseResponseExtended>> GetListOfTasksCount(ProcessRequest responses);
        Task<Process> GetTaskByTaskKey(ProcessRequest responses);
        Task<IList<ProcessComponent>> GetProcessComponents(ProcessRequest responses);
        Task<bool> DeleteComponents(ProcessRequest responses);
        Task<IList<ProcessRemark>> GetProcessRemarksByProcess(ProcessRequest responses);
        Task<bool> SaveRemarks(ProcessRemark responses);
        Task<bool> UpdateTask(TaskInsertUpdate responses);
        Task<bool> CreateProcessComponent(ProcessComponent responses);
        Task<IList<Process>> GetSubTaskByTaskKey(ProcessRequest responses);
        Task<bool> CreateTask(TaskInsertUpdate responses);
        Task<string> GetNextTaskID(ProcessRequest responses);
        Task<IList<Process>> GetTodo_ListviewSelectWeb(ProcessRequest responses);
        Task<IList<CheckList>> ToDoChkLst(ProcessRequest responses);
        Task<bool> CreateUpdateCheckList(CheckList responses);
    }
}
