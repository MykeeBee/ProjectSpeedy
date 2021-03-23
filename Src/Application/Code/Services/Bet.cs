using System;
using System.Text.Json;

namespace ProjectSpeedy.Services
{
    /// <summary>
    /// All bet related services.
    /// </summary>
    public class Bet : IBet
    {
        /// <summary>
        /// Contains helper functions needed for all services to work
        /// </summary>
        private readonly IServiceBase _serviceBase;

        /// <summary>
        /// All bet related services.
        /// </summary>
        /// <param name="serviceBase">Contains helper functions needed for all services to work.</param>
        public Bet(IServiceBase serviceBase)
        {
            this._serviceBase = serviceBase;
        }

        /// <inheritdoc />
        public async System.Threading.Tasks.Task<bool> CreateAsync(string projectId, string problemId, Models.Bet.BetNew form)
        {
            // TODO We need to ensure that the project exists with the supplied Id.

            // TODO We need to ensure that there is not another problem with the same name.

            // The new project object
            var newBet = new ProjectSpeedy.Models.Bet.Bet()
            {
                Name = form.Name,
                Created = DateTime.UtcNow,
                ProjectId = "project:" + projectId,
                ProblemId = "problem:" + problemId
            };

            // Creates the project and checks if the id is returned.
            var newId = await this._serviceBase.DocumetCreate(newBet, "bet");
            return !string.IsNullOrWhiteSpace(newId);
        }

        /// <inheritdoc />
        public bool Delete(string projectId, string betId)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public async System.Threading.Tasks.Task<Models.Bet.Bet> GetAsync(string projectId, string problemId, string betId)
        {
            // Id of the bet in couchDb.
            var couchProjectId = "bet:" + betId;

            // Gets the base bet.
            var viewData = await this._serviceBase.GetDocument(couchProjectId);
            using var responseStream = await viewData.ReadAsStreamAsync();
            var bet = await JsonSerializer.DeserializeAsync<ProjectSpeedy.Models.Bet.Bet>(responseStream);
            return bet;
        }

        /// <inheritdoc />
        public bool Update(string projectId, string problemId, string betId, Models.Bet.BetNew form)
        {
            throw new System.NotImplementedException();
        }
    }
}