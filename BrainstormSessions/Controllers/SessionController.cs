using System.Threading.Tasks;
using BrainstormSessions.Core.Interfaces;
using BrainstormSessions.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace BrainstormSessions.Controllers
{
    public class SessionController : Controller
    {
        private readonly IBrainstormSessionRepository _sessionRepository;

        public SessionController(IBrainstormSessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }

        public async Task<IActionResult> Index(int? id)
        {
            if (!id.HasValue)
            {
                Log.Warning("Id parameter is null. Redirecting to Home Index.");

                return RedirectToAction(actionName: nameof(Index),
                    controllerName: "Home");
            }

            Log.Debug("Getting session by id {sessionId}.", id.Value);
            var session = await _sessionRepository.GetByIdAsync(id.Value);
            if (session == null)
            {
                Log.Error("Session not found with id {sessionId}.", id.Value);

                return Content("Session not found.");
            }

            var viewModel = new StormSessionViewModel()
            {
                DateCreated = session.DateCreated,
                Name = session.Name,
                Id = session.Id
            };

            return View(viewModel);
        }
    }
}
