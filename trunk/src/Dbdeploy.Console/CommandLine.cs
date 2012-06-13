using System;
using System.IO;
using Net.Sf.Dbdeploy.Configuration;
using Net.Sf.Dbdeploy.Database;
using Net.Sf.Dbdeploy.Exceptions;

namespace Net.Sf.Dbdeploy
{
    public class CommandLine
    {
        private const int LastVersionChangeToApply = Int32.MaxValue;

        public static void Main(string[] args)
        {
            try
            {
                var commandLineArgumentsParser = new CommandLineArgumentsParser();
                ParsedArguments parsedArguments = commandLineArgumentsParser.ParseArgs(args);

                var printScreenFactory = new PrintScreenFactory();

                var config = new CommandLineArgsConfiguration(parsedArguments);
                var factory = new DbmsFactory(config.DbType, config.DbConnectionString);
                var databaseSchemaVersion = new DatabaseSchemaVersionManager(factory, config.DbDeltaSet, config.CurrentDbVersion, config.TableName, config.ChangeOwner);
                
                var directoryInfo = new DirectoryInfo(parsedArguments.GetScriptFilesFolderOrDefaultFolder());
                TextWriter outputPrintStream = printScreenFactory.GetDoPrintStream(parsedArguments);
                var dbmsSyntax = factory.CreateDbmsSyntax(config.ChangeOwner);
                var useTransaction = config.UseTransaction;
                TextWriter undoOutputPrintStream = printScreenFactory.GetUndoPrintStream(parsedArguments);

                var toPrintStreamDeployer = new ToPrintStreamDeployer(databaseSchemaVersion, directoryInfo, outputPrintStream, dbmsSyntax, useTransaction, undoOutputPrintStream);

                toPrintStreamDeployer.DoDeploy(LastVersionChangeToApply);

                printScreenFactory.ClosePrintStream(outputPrintStream);
                printScreenFactory.ClosePrintStream(undoOutputPrintStream);
            }
            catch (DbDeployException ex)
            {
                Console.Error.WriteLine(ex.Message);
                Environment.Exit(1);
            }

            catch (Exception ex)
            {
                Console.Error.WriteLine("Failed to apply changes: " + ex);
                Console.Error.WriteLine(ex.StackTrace);
                Environment.Exit(2);
            }
        }
    }
}