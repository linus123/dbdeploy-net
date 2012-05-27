using System;
using System.Collections.Generic;
using System.IO;
using Net.Sf.Dbdeploy.Database;
using Net.Sf.Dbdeploy.Scripts;

namespace Net.Sf.Dbdeploy
{
    public class ToPrintStreamDeployer
    {
        private readonly DirectoryInfo dir;
        private readonly TextWriter doOutputPrintStream;
        private readonly TextWriter undoOutputPrintStream;
        private readonly DatabaseSchemaVersionManager schemaManager;
        private readonly IDbmsSyntax dbmsSyntax;
    	private readonly bool useTransaction;

    	public ToPrintStreamDeployer(DatabaseSchemaVersionManager schemaManager, DirectoryInfo dir,
                                     TextWriter outputPrintStream,
                                     IDbmsSyntax dbmsSyntax, bool useTransaction, TextWriter undoOutputPrintStream)
        {
            this.schemaManager = schemaManager;
            this.dir = dir;
            doOutputPrintStream = outputPrintStream;
            this.dbmsSyntax = dbmsSyntax;
        	this.useTransaction = useTransaction;
        	this.undoOutputPrintStream = undoOutputPrintStream;
        }

        public void DoDeploy(Int64 lastChangeToApply)
        {
            Console.Out.WriteLine("dbdeploy v2.12");

            List<ChangeScript> changeScripts = new DirectoryScanner().GetChangeScriptsForDirectory(dir);
            ChangeScriptRepository repository = new ChangeScriptRepository(changeScripts);
            List<Int64> appliedChanges = schemaManager.GetAppliedChangeNumbers();

            GenerateChangeScripts(repository, lastChangeToApply, appliedChanges);
            if (undoOutputPrintStream != null)
            {
                GenerateUndoChangeScripts(repository, lastChangeToApply, appliedChanges);
            }
        }

        private void GenerateChangeScripts(ChangeScriptRepository repository, Int64 lastChangeToApply, List<Int64> appliedChanges)
        {
            ChangeScriptExecuter doScriptExecuter = new ChangeScriptExecuter(doOutputPrintStream, dbmsSyntax, useTransaction);
            Controller doController = new Controller(schemaManager, repository, doScriptExecuter);
            doController.ProcessDoChangeScripts(lastChangeToApply, appliedChanges);
            doOutputPrintStream.Flush();
        }

        private void GenerateUndoChangeScripts(ChangeScriptRepository repository, Int64 lastChangeToApply, List<Int64> appliedChanges)
        {
            ChangeScriptExecuter undoScriptExecuter = new ChangeScriptExecuter(undoOutputPrintStream, dbmsSyntax, useTransaction);
            Controller undoController = new Controller(schemaManager, repository, undoScriptExecuter);
            undoController.ProcessUndoChangeScripts(lastChangeToApply, appliedChanges);
            undoOutputPrintStream.Flush();
        }
    }
}