
namespace WorkManagementSystem.Features.Publish.Command
{
    public class GetNoteWorkDispatchHandler : ICommandHandler<NoteCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetNoteWorkDispatchHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> ExecuteAsync(NoteCommand command, CancellationToken ct)
        {
            try
            {
                var noteRepo = _unitOfWork.GetRepository<Note>();
                var note = new Note
                {
                    WorkFlow = command.WorkFlow,
                    Created = DateTime.Now,
                    Notes = command.Notes,
                    UserId = command.UserId,
                };
                await noteRepo.AddAsync(note);
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
           
            
        }
    }
}
