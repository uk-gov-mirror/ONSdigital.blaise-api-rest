﻿using System.Threading.Tasks;
using Blaise.Api.Tests.Helpers.Cloud;
using Blaise.Api.Tests.Helpers.Configuration;
using Blaise.Api.Tests.Helpers.Instrument;
using Blaise.Api.Tests.Helpers.RestApi;
using TechTalk.SpecFlow;

namespace Blaise.Api.Tests.Behaviour.Steps
{
    [Binding]
    public sealed class DeployQuestionnaireSteps
    {
        private readonly ScenarioContext _scenarioContext;

        public DeployQuestionnaireSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"there is a questionnaire available in a bucket")]
        public void GivenThereIsAnQuestionnaireAvailableInABucket()
        {
            CloudStorageHelper.GetInstance().UploadToBucket(
                BlaiseConfigurationHelper.InstrumentBucketPath,
                BlaiseConfigurationHelper.InstrumentPackage);
        }

        [When(@"the API is called to deploy the questionnaire")]
        public async Task WhenTheApiIsCalledToDeployTheQuestionnaire()
        {
            await RestApiHelper.GetInstance().DeployQuestionnaire(
                RestApiConfigurationHelper.InstrumentsUrl,
                BlaiseConfigurationHelper.InstrumentBucketPath,
                BlaiseConfigurationHelper.InstrumentPackage);
        }


        [AfterScenario("deploy")]
        public void CleanUpScenario()
        {
            InstrumentHelper.GetInstance().UninstallSurvey();

            CloudStorageHelper.GetInstance().DeleteFromBucket(
                BlaiseConfigurationHelper.InstrumentBucketPath,
                BlaiseConfigurationHelper.InstrumentPackage);
        }
    }
}
