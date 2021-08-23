using System;
using System.Transactions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace Milabowl.Test
{
    public class RollbackAttribute: Attribute, ITestAction
    {
        private TransactionScope _transactionScope;

        public void BeforeTest(ITest test)
        {
            this._transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }, TransactionScopeAsyncFlowOption.Enabled);
        }

        public void AfterTest(ITest test)
        {
            this._transactionScope.Dispose();
        }

        public ActionTargets Targets => ActionTargets.Test;
    }
}
