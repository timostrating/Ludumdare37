using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class CodeManager : MonoBehaviour {

    public GameObject errorPanel;
    public Text errorText;



    public void Run( Text textfield ) {
        Run(textfield.text);
    }

    public void Run( string code ) {
        var assembly = SetupAssembly(code); ;

        var method = assembly.GetType("CodeRunner").GetMethod("Run");
        //		var del = (Action)Delegate.CreateDelegate(typeof(Action), method);   // this is needed if we want to call static classes
        //		del.Invoke();

        var o = Activator.CreateInstance( assembly.GetType("CodeRunner") );
        var result = method.Invoke(o, null);
    }

    private Assembly SetupAssembly(String code) {
        return Compile(@"
		using UnityEngine;

		public class CodeRunner {
            Hero hero;
            
            public void Run() { 
                hero = Hero.instance;
                hero.Inform();                  // let's say hello
                RunCode();                      // Register Commands from player's input 
                hero.RunRegisterdCommands();
            }

			private void RunCode() { 
                "+preProcessCode(code)+ @" 
            }
        }");
    }

    private String preProcessCode(String code) {
        //TODO: we should Pre Process all Code to fix common mistakes like not ending with ;
        return code;
    }

    private Assembly Compile( string source ) {
        var provider = new CSharpCodeProvider();
        var param = new CompilerParameters();

        // Add ALL of the assembly references
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
            param.ReferencedAssemblies.Add(assembly.Location);
        }

        //param.ReferencedAssemblies.Add("System.dll");
        //param.ReferencedAssemblies.Add("CSharp.dll");
        //param.ReferencedAssemblies.Add("UnityEngines.dll");

        // Generate a dll in memory
        param.GenerateExecutable = false;
        param.GenerateInMemory = true;

        // Compile the source
        var result = provider.CompileAssemblyFromSource(param, source);

        if (result.Errors.Count > 0) {
            var msg = new StringBuilder();
            foreach (CompilerError error in result.Errors) {
                msg.AppendFormat("Error ({0}): {1}\n", error.ErrorNumber, error.ErrorText);
            }
            ShowError(msg.ToString());
            throw new Exception(msg.ToString());
        }

        // Return the assembly
        return result.CompiledAssembly;
    }

    private void ShowError(string msg) {
        errorText.text = msg;
        errorPanel.SetActive( true );
    }
}