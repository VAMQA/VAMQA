﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Platform.TestAutomationFramework.UIAutomation.Commands.Interactions
{
    using System;
    using VM.Platform.TestAutomationFramework.Core;
    using VM.Platform.TestAutomationFramework.Core.Commands;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using VM.Platform.TestAutomationFramework.Core.Exceptions;

    [CanParse(@"^WaitForAjax\s*\{\s*(?<LogicalFieldName>.*?)\s*\|\|\s*(?<LogicalFieldValue>.*?)\s*\}$")]
    public class WaitForAjaxCommand : InteractionCommand
    {
        public WaitForAjaxCommand(ExecutionParameters executionParameters, IUiAdapter uiAdapter)
            : base(executionParameters, uiAdapter)
        {
        }

        public override void Execute(TestRunContext context)
        {
            using (new FrameHandler(this.uiAdapter, context, this.logicalFieldName))
            {
                try
                {
                    var controlDefinition = context.ControlMap[context.CurrentPage][this.logicalFieldName];
                    this.uiAdapter.WaitForAjaxCallComplete(controlDefinition);
                    this.uiAdapter.WaitForElementToBeClickable(controlDefinition);
                }
                catch (Exception ex)
                {
                    this.uiAdapter.TakeScreenshot();
                    //Report the Error to HTML REPORT
                    ReportFailureToHtml ReportError = new ReportFailureToHtml();
                    context = ReportError.Run(context, string.Format("Error for {0}", this.logicalFieldName), string.Format("Waited, but the {0} control never appeared.", this.logicalFieldName), TestResult.Fail);


                    throw new WorkflowFailedException(
                        string.Format("Waited, but the {0} control never appeared.", this.logicalFieldName), ex);
                }
            }
        }
    }
}
