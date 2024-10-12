using OfficePass.Areas.Adminka.Domain.Repositories;
using OfficePass.Domain.Entities;
using OfficePass.Domain.Repositories;
using OfficePass.Domain.Repositories.Abstract;
using OfficePass.Enums;

namespace OfficePass.Services
{
    public class GroupService : IGroupService
    {
        private readonly GroupRepository repository;

        public GroupService(GroupRepository repository)
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));
            this.repository = repository;
        }

        public async Task<IBaseResponse<Group>> CreateGroup(Group model)
        {
            try
            {
                var _model = new Group
                {
                    Name = model.Name,
                    Description = model.Description,
                };
                await repository.Create(_model);
                return new BaseResponse<Group>
                {
                    StatusCode = StatusCode.OK,
                    Data = _model,
                    Description = "Подразделение успешно создано"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Group>
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"[CreateGroup]: {ex.Message}"
                };
            }
        }

        public async Task<IBaseResponse<bool>> DeleteGroup(int id)
        {
            try
            {
                var _model = repository.GetAll().FirstOrDefault(x => x.Id == id);
                if (_model == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.NotFound,
                        Description = "Подразделение не найдено"
                    };
                }

                await repository.Delete(_model);
                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = StatusCode.OK,
                    Description = $"Подразделение {_model.Name} успешно удалено"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"[DeleteGroup]: {ex.Message}"
                };
            }
        }

        public async Task<IBaseResponse<List<Group>>> GetGroups()
        {
            try
            {
                var _models = repository.GetAll().ToList();
                if (_models == null)
                {
                    return new BaseResponse<List<Group>>
                    {
                        Description = "Цеха или отделы не найдены",
                        StatusCode = StatusCode.NotFound
                    };
                }

                return new BaseResponse<List<Group>>
                {
                    Data = _models,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Group>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }

        }

        public IBaseResponse<Group> GetGroupById(int id)
        {
            try
            {
                var _models = repository.GetAll().FirstOrDefault(x => x.Id == id);
                if (_models == null)
                {
                    return new BaseResponse<Group>()
                    {
                        StatusCode = StatusCode.NotFound,
                        Description = "Цех или отдел не найден"
                    };
                }
                return new BaseResponse<Group>()
                {
                    StatusCode = StatusCode.OK,
                    Data = _models
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Group>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"[GetGroupById]: {ex.Message}"
                };
            }

        }

        public async Task<IBaseResponse<bool>> UpdateGroup(Group model)
        {
            try
            {
                var _model = repository.GetAll().FirstOrDefault(x => x.Id == model.Id);
                if (_model == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.NotFound,
                        Description = "Подразделение не найдено"
                    };
                }
                _model.Name = model.Name;
                _model.Description = model.Description;

                var result = await repository.Update(_model);
                if (result)
                    return new BaseResponse<bool>()
                    {
                        Data = true,
                        StatusCode = StatusCode.OK,
                        Description = $"Подразделение \"{_model.Name}\" успешно изменено"
                    };
                else
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.UpdateDBError,
                        Description = $"Возникли проблемы при изменении подразделения \"{_model.Name}\""
                    };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Data = false,
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"[UpdateGroup]: {ex.Message}"
                };
            }

        }
    }
}
