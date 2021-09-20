using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aplication.ClinicHistoryApp;
using Aplication.ClinicHistoryApp.BackgroundMedicalApp;
using Aplication.ClinicHistoryApp.BackgroundOralApp;
using Aplication.ClinicHistoryApp.Dtos;
using Aplication.ClinicHistoryApp.OralRadiographyApp;
using Aplication.ClinicHistoryApp.PatientEvolutionApp;
using Aplication.ClinicHistoryApp.TreamentPlanApp;
using Domine;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class ClinicHistoryController : myControllerBase
    {
        
        [HttpGet("users/{id}")]
        public async Task<ActionResult<clinicHistoryDto>> GetActionAsync(string id){
            return await mediator.Send(new getClinicHistoryWithUser.OneClinicHistoryUser{Id = id});
        }

        [HttpGet("backgroundOral")]
        public async Task<ActionResult<List<BackgroundOral>>> Get(){

            return await mediator.Send(new getAllBackgroundOral.ListbackgroundOral());
        }
        [HttpGet("backgroundMedical")]
        public async Task<ActionResult<List<BackgroundMedical>>> GetTask(){

            return await mediator.Send(new getAllBackgroundMedical.ListbackgroundMedical());
        }

        [HttpPost("addClinicHistory")]
        public async Task<ActionResult<Unit>> CreateClinicHistory(PostClinicHistory.Execute data){
            return await mediator.Send(data);
        }

        [HttpPost("addEvolution")]
        public async Task<ActionResult<Unit>> CreateEvolution(PostPatientEvolution.Execute data){
            return await mediator.Send(data);
        }

        [HttpPost("addRadiography")]
        public async Task<ActionResult<Unit>> CreateOralRadiography(PostOralRadiography.Execute data){
            return await mediator.Send(data);
        }

        [HttpPost("addTreamentPlan")]
        public async Task<ActionResult<Unit>> CreateTreamentPlan(PostTreamentPlan.Execute data){
            return await mediator.Send(data);
        }

        [HttpPut("edit/{id}")]
        public async Task<ActionResult<Unit>> putClinicHistory(Guid id, putClinicHistory.Execute data){
            data.Id = id;
            return await mediator.Send(data);
        }

       [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> deleteClinicHistory(Guid id, deleteClinicHistory.Execute data){
            data.Id = id;
            return await mediator.Send(data);
        }

        
    }
}