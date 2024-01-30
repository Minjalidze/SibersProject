using SibersProject.ApiModels.DTOs.EntitiesDTO.TaskItem;
using SibersProject.ApiModels.Response.Features;
using SibersProject.ApiModels.Response.Interfaces;
using SibersProject.DataAL.Repository.Interfaces;
using SibersProject.MainDomain.Models.Entities;
using SibersProject.MainDomain.Models.Enums;
using SibersProject.Services.Helpers;
using SibersProject.Services.Services.Interfaces;
using SibersProject.Validator;
using SibersProject.ApiModels.DTOs.EntitiesDTO.TaskItem;
using Microsoft.AspNetCore.Identity;

namespace SibersProject.Services.Services.Implementations
{
    public class TaskItemService : ITaskItemService
    {
        private readonly ITaskItemRepository _taskItemRepository;
        private readonly UserManager<Employee> _userManager;

        public TaskItemService(ITaskItemRepository taskItemRepository, UserManager<Employee> userManager)
        {
            _taskItemRepository = taskItemRepository;
            _userManager = userManager;
        }

        public async Task<IBaseResponse<Guid>> CreateAsync(CreateTaskItemDTO createTaskItemDto)
        {
            try
            {
                var taskItem = new TaskItem
                {
                    Name = createTaskItemDto.Name,
                    Comment = createTaskItemDto.Comment,
                    Status = createTaskItemDto.Status,
                    Priority = createTaskItemDto.Priority,
                };


                if (await _userManager.FindByIdAsync(createTaskItemDto.AuthorId) != null)
                    taskItem.AuthorId = createTaskItemDto.AuthorId;

                if (await _userManager.FindByIdAsync(createTaskItemDto.ExecutorId) != null)
                    taskItem.ExecutorId = createTaskItemDto.ExecutorId;


                await _taskItemRepository.Create(taskItem);

                return ResponseFactory<Guid>.CreateSuccessResponse(taskItem.Id);
            }
            catch (ArgumentNullException argNullException)
            {
                return ResponseFactory<Guid>.CreateNotFoundResponse(argNullException);
            }
            catch (Exception exception)
            {
                return ResponseFactory<Guid>.CreateErrorResponse(exception);
            }
        }

        public async Task<IBaseResponse<bool>> DeleteAsync(Guid id)
        {
            try
            {
                ObjectValidator<Guid>.CheckIsNotNullObject(id);

                await _taskItemRepository.DeleteByIdAsync(id);

                return ResponseFactory<bool>.CreateSuccessResponse(true);
            }
            catch (ArgumentNullException argNullException)
            {
                return ResponseFactory<bool>.CreateNotFoundResponse(argNullException);
            }
            catch (Exception exception)
            {
                return ResponseFactory<bool>.CreateErrorResponse(exception);
            }
        }

        public async Task<IBaseResponse<IEnumerable<TaskItem>>> GetAsync(FilterTaskItemDTO? taskItemFilterDto, Sort? sortOrder)
        {
            try
            {
                ObjectValidator<FilterTaskItemDTO>.CheckIsNotNullObject(taskItemFilterDto);

                var filter = FilterHelper.CreateTaskItemFilter(taskItemFilterDto);

                var taskItems = await _taskItemRepository.GetFilteredTaskItemsAsync(filter);

                if (sortOrder.HasValue)
                {
                    taskItems = sortOrder switch
                    {
                        Sort.NameDesc => taskItems.OrderByDescending(t => t.Name),
                        Sort.NameAsc => taskItems.OrderBy(t => t.Name),
                        Sort.PriorityDesc => taskItems.OrderByDescending(t => t.Priority),
                        Sort.PriorityAsc => taskItems.OrderBy(t => t.Priority),
                        _ => taskItems,
                    };
                }

                return ResponseFactory<IEnumerable<TaskItem>>.CreateSuccessResponse(taskItems);
            }
            catch (ArgumentNullException argNullException)
            {
                return ResponseFactory<IEnumerable<TaskItem>>.CreateNotFoundResponse(argNullException);
            }
            catch (Exception exception)
            {
                return ResponseFactory<IEnumerable<TaskItem>>.CreateErrorResponse(exception);
            }
        }

        public async Task<IBaseResponse<TaskItem>> GetByIdAsync(Guid id)
        {
            try
            {
                ObjectValidator<Guid>.CheckIsNotNullObject(id);

                var taskItem = await _taskItemRepository.ReadByIdAsync(id);

                return ResponseFactory<TaskItem>.CreateSuccessResponse(taskItem);
            }
            catch (ArgumentNullException argNullException)
            {
                return ResponseFactory<TaskItem>.CreateNotFoundResponse(argNullException);
            }
            catch (Exception exception)
            {
                return ResponseFactory<TaskItem>.CreateErrorResponse(exception);
            }
        }

        public async Task<IBaseResponse<bool>> UpdateAuthorAsync(Guid id, UpdateTaskItemWithAuthorDTO taskItemDto)
        {
            try
            {
                ObjectValidator<UpdateTaskItemWithAuthorDTO>.CheckIsNotNullObject(taskItemDto);

                var taskItem = await _taskItemRepository.GetFilteredTaskItemByIdAsync(id);

                taskItem.Name = taskItemDto.Name;
                taskItem.Comment = taskItemDto.Comment;
                taskItem.Status = taskItemDto.Status;
                taskItem.Priority = taskItemDto.Priority;
                taskItem.AuthorId = taskItemDto.AuthorId;

                await _taskItemRepository.UpdateAsync(taskItem);

                return ResponseFactory<bool>.CreateSuccessResponse(true);
            }
            catch (ArgumentNullException argNullException)
            {
                return ResponseFactory<bool>.CreateNotFoundResponse(argNullException);
            }
            catch (Exception exception)
            {
                return ResponseFactory<bool>.CreateErrorResponse(exception);
            }
        }

        public async Task<IBaseResponse<bool>> UpdateExecutorAsync(Guid id, UpdateTaskItemWithExecutorDTO taskItemDto)
        {
            try
            {
                ObjectValidator<UpdateTaskItemWithExecutorDTO>.CheckIsNotNullObject(taskItemDto);

                var taskItem = await _taskItemRepository.GetFilteredTaskItemByIdAsync(id);

                taskItem.Name = taskItemDto.Name;
                taskItem.Comment = taskItemDto.Comment;
                taskItem.Status = taskItemDto.Status;
                taskItem.Priority = taskItemDto.Priority;
                taskItem.ExecutorId = taskItemDto.ExecutorId;

                await _taskItemRepository.UpdateAsync(taskItem);

                return ResponseFactory<bool>.CreateSuccessResponse(true);
            }
            catch (ArgumentNullException argNullException)
            {
                return ResponseFactory<bool>.CreateNotFoundResponse(argNullException);
            }
            catch (Exception exception)
            {
                return ResponseFactory<bool>.CreateErrorResponse(exception);
            }
        }
    }
}
