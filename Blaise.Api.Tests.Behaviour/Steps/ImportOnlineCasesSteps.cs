using System.Net;
using System.Threading.Tasks;
using Blaise.Api.Tests.Helpers.Case;
using Blaise.Api.Tests.Helpers.Configuration;
using Blaise.Api.Tests.Helpers.Enums;
using Blaise.Api.Tests.Helpers.Files;
using Blaise.Api.Tests.Helpers.Instrument;
using Blaise.Api.Tests.Helpers.RestApi;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Blaise.Api.Tests.Behaviour.Steps
{
    [Binding]
    public class ImportOnlineCasesSteps
    {
        private readonly ScenarioContext _scenarioContext;

        public ImportOnlineCasesSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeFeature("onlinedata")]
        public static void SetupUpFeature()
        {
            InstrumentHelper.GetInstance().InstallSurvey();
        }

        [Given(@"there is a not a online file available")]
        public void GivenThereIsANotAOnlineFileAvailable()
        {
        }
        
        [Given(@"there is a online file that contains a case that is complete")]
        public async Task GivenThereIsAOnlineDataFileThatContainsACaseThatIsCompleteAsync()
        {
            await GivenThereIsAnOnlineFileThatContainsACaseWithTheOutcomeCodeAsync(110);
        }

        [Given(@"there is a online file that contains a case that is partially complete")]
        public async Task GivenThereIsAOnlineFileThatContainsACaseThatIsPartiallyComplete()
        {
            await GivenThereIsAnOnlineFileThatContainsACaseWithTheOutcomeCodeAsync(210);
        }

        [Given(@"there is a online file that contains a case that has not been started")]
        public async Task GivenThereIsAOnlineFileThatContainsACaseThatHasNotBeenStarted()
        {
            await GivenThereIsAnOnlineFileThatContainsACaseWithTheOutcomeCodeAsync(0);
        }

        [Given(@"there is an online file that contains a case with the outcome code '(.*)'")]
        public async Task GivenThereIsAnOnlineFileThatContainsACaseWithTheOutcomeCodeAsync(int outcomeCode)
        {
            var primaryKey = await OnlineFileHelper.GetInstance().CreateCaseInOnlineFileAsync(outcomeCode);
            _scenarioContext.Set(primaryKey,"primaryKey");
        }

        [Given(@"the same case exists in Blaise that is complete")]
        public void GivenTheSameCaseExistsInBlaiseThatIsComplete()
        {
            GivenTheSameCaseExistsInBlaiseWithTheOutcomeCode(110);
        }

        [Given(@"the same case exists in Blaise that is partially complete")]
        public void GivenTheSameCaseExistsInBlaiseThatIsPartiallyComplete()
        {
            GivenTheSameCaseExistsInBlaiseWithTheOutcomeCode(210);
        }

        [Given(@"the same case exists in Blaise with the outcome code '(.*)'")]
        public void GivenTheSameCaseExistsInBlaiseWithTheOutcomeCode(int outcomeCode)
        {
            var primaryKey = _scenarioContext.Get<string>("primaryKey");
            CaseHelper.GetInstance().CreateCaseInBlaise(primaryKey, outcomeCode, ModeType.Tel);
        }
        
        [When(@"the online file is imported")]
        public async Task WhenTheOnlineFileIsImported()
        {
           var statusCode = await RestApiHelper.GetInstance().ImportOnlineCases(RestApiConfigurationHelper.InstrumentDataUrl,
                BlaiseConfigurationHelper.InstrumentName);

            Assert.AreEqual(HttpStatusCode.Created, statusCode);
        }

        [Then(@"the existing blaise case is overwritten with the online case")]
        public void ThenTheExistingBlaiseCaseIsOverwrittenWithTheOnlineCase()
        {
            var primaryKey = _scenarioContext.Get<string>("primaryKey");
            var modeType = CaseHelper.GetInstance().GetMode(primaryKey);
            Assert.AreEqual(ModeType.Web, modeType);
        }

        [Then(@"the existing blaise case is kept")]
        public void ThenTheExistingBlaiseCaseIsKept()
        {
            var primaryKey = _scenarioContext.Get<string>("primaryKey");
            var modeType = CaseHelper.GetInstance().GetMode(primaryKey);
            Assert.AreEqual(ModeType.Tel, modeType);
        }

        [Given(@"there is a online file that contains '(.*)' cases")]
        public async Task GivenThereIsAOnlineFileThatContainsCases(int numberOfCases)
        {
            await OnlineFileHelper.GetInstance().CreateCasesInOnlineFileAsync(numberOfCases);
        }
        
        [Given(@"blaise contains no cases")]
        public void GivenTheBlaiseDatabaseIsEmpty()
        {
            CaseHelper.GetInstance().DeleteCases();
        }

        [Given(@"blaise contains '(.*)' cases")]
        public void GivenBlaiseContainsCases(int numberOfCases)
        {
            CaseHelper.GetInstance().CreateCasesInBlaise(numberOfCases);
        }


        [Then(@"blaise will contain no cases")]
        public void ThenBlaiseWillContainNoCases()
        {
            ThenCasesWillBeImportedIntoBlaise(0);
        }

        [Then(@"blaise will contain '(.*)' cases")]
        public void ThenCasesWillBeImportedIntoBlaise(int numberOfCases)
        {
            var numberOfCasesInBlaise = CaseHelper.GetInstance().NumberOfCasesInInstrument();

            Assert.AreEqual(numberOfCases, numberOfCasesInBlaise);
        }

        [AfterScenario("onlinedata")]
        public static async Task CleanUpScenario()
        {
            CaseHelper.GetInstance().DeleteCases();
            await OnlineFileHelper.GetInstance().CleanUpOnlineFiles();
        }
        
        [AfterFeature("onlinedata")]
        public static void CleanUpFeature()
        {
            InstrumentHelper.GetInstance().UninstallSurvey();
        }
    }
}
