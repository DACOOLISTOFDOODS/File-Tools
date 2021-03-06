using Microsoft.Win32;
namespace GUI;

public partial class Form1 : Form
{

    public void reg(bool y_n, bool cascade, string name, string[] values, string[] actions, string location, string icon, string pos)
    {
        if (!y_n)
        { // remove
            if (!cascade)
            {
                Registry.ClassesRoot.DeleteSubKey(location + "\\shell\\" + name + "\\command");
                Registry.ClassesRoot.DeleteSubKey(location + "\\shell\\" + name);
            }
            else
            {
                for (int i = 0; i < values.Length; i++)
                {
                    Registry.ClassesRoot.DeleteSubKey(location + "\\shell\\" + name + "\\shell\\" + values[i] + "\\command");
                    Registry.ClassesRoot.DeleteSubKey(location + "\\shell\\" + name + "\\shell\\" + values[i]);
                }
                Registry.ClassesRoot.DeleteSubKey(location + "\\shell\\" + name + "\\shell");
                Registry.ClassesRoot.DeleteSubKey(location + "\\shell\\" + name);
            }
        }
        else
        { // add
            RegistryKey reg;
            if (cascade)
            {

                reg = Registry.ClassesRoot.CreateSubKey(location + "\\shell\\" + name);
                Registry.ClassesRoot.CreateSubKey(location + "\\shell\\" + name + "\\shell");
                reg.SetValue("MUIVerb", name);
                reg.SetValue("subcommands", "");
                reg.SetValue("Icon", icon);
                if(location != "Directory\\Background") reg.SetValue("Position", pos);
                for (int i = 0; i < values.Length; i++)
                {
                    Registry.ClassesRoot.CreateSubKey(location + "\\shell\\" + name + "\\shell\\" + values[i]);
                    reg = Registry.ClassesRoot.CreateSubKey(location + "\\shell\\" + name + "\\shell\\" + values[i] + "\\command");
                    reg.SetValue("", actions[i]);
                }
            }
            else
            {
                Registry.ClassesRoot.CreateSubKey(location + "\\shell\\" + name);
                reg = Registry.ClassesRoot.CreateSubKey(location + "\\shell\\" + name);
                reg.SetValue("Icon", icon);
                reg.SetValue("Position", pos);
                reg = Registry.ClassesRoot.CreateSubKey(location + "\\shell\\" + name + "\\command");
                reg.SetValue("", actions[0]);
            }
        }
    }



    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {

    }

    private void yes_rmdir_Click(object sender, EventArgs e)
    {
        reg(true, false, "RMDIR", null, new[] { "cmd.exe /c rmdir \"%1\" /Q /S" }, "Directory", "%SystemRoot%\\System32\\shell32.dll,-240", "Bottom");
    }

    private void remove_rmdir_Click(object sender, EventArgs e)
    {
        try { reg(false, false, "RMDIR", null, null, "Directory", null, null); } catch(Exception)
        {

        };
    }

    private void yes_copy_Click(object sender, EventArgs e)
    {
        string location = System.Reflection.Assembly.GetEntryAssembly().Location;
        string dir = '\"' + location.Substring(0, location.LastIndexOf("GUI.dll")) + "invisible.exe\"";
        string[] actions = new[] { dir + "\"copy\" \"1\" \"%1\"", dir + "\"copy\" \"2\" \"%1\"" };
        reg(true, true, "Robocopy", new[] { "Copy 1", "Copy 2" }, actions, "Directory", "%SystemRoot%\\System32\\shell32.dll,-16762", "Top");
        reg(true, true, "Robopaste", new[] { "Paste 1", "Paste 2" }, new[] { dir + " \"paste\" \"1\" \"%w\"", dir + " \"paste\" \"2\" \"%w\"" }, "Directory\\Background", "%SystemRoot%\\System32\\shell32.dll,-16763", "Top");
    }

    private void no_copy_Click(object sender, EventArgs e)
    {
        try{ reg(false, true, "Robocopy", new[] { "Copy 1", "Copy 2"}, null, "Directory", null, null);
        reg(false, true, "Robopaste", new[] { "Paste 1", "Paste 2" }, null, "Directory\\Background", null, null); } 
        catch(Exception)
        {

        };
    }

    private void yes_sym_Click(object sender, EventArgs e)
    {
        string location = System.Reflection.Assembly.GetEntryAssembly().Location;
        string dir = '\"' + location.Substring(0, location.LastIndexOf("GUI.dll")) + "sym.exe\" \"%1\"";
        reg(true, true, "Symlink", new[] { "Batch Symlink", "Batch Symlink but copy executables", "Symlink" }, new[] { dir + " \"0\"", dir + " \"copy\"", dir + " \"singleD\"" }, "Directory", "%SystemRoot%\\System32\\shell32.dll,-16806", "Top");
        reg(true, false, "Symlink", new[] { "Symlink" }, new[] { dir +" \"singleF\"" }, "*", "%SystemRoot%\\System32\\shell32.dll,-16806", "Middle");
    }

    private void no_sym_Click(object sender, EventArgs e)
    {
        try { reg(false, true, "Symlink", null, null, "Directory", null, null);  
        reg(false, false, "Symlink", null, null, "*", null, null);
        } catch(Exception)
        {

        };
    }

    private void yes_note_Click(object sender, EventArgs e)
    {
        string location = System.Reflection.Assembly.GetEntryAssembly().Location;
        string dir = location.Substring(0, location.LastIndexOf("GUI.dll")) + "win11notepad.ico";
        reg(true, false, "Open with Notepad", null, new[] { "notepad.exe %1" }, "*", dir, "Middle");
    }

    private void no_note_Click(object sender, EventArgs e)
    {
        try { reg(false, false, "Open with Notepad", null, null, "*", null, null); } catch (Exception) {
         };
    }

    private void yes_compact_Click(object sender, EventArgs e)
    {
        reg(true, false, "Compact", null, new[] { "compact.exe \"%1\\*.*\" /Q /C /S" }, "Directory", "%systemroot%\\system32\\imageres.dll,-175", "Middle");
    }

    private void no_compact_Click(object sender, EventArgs e)
    {
        try { reg(false, false, "Compact", null, null, "Directory", null, null);
        }
        catch (Exception){};
    }

    private void yes_path_Click(object sender, EventArgs e)
    {
        string dir = '\"' + System.Reflection.Assembly.GetEntryAssembly().Location.Substring(0, System.Reflection.Assembly.GetEntryAssembly().Location.LastIndexOf("GUI.dll")) + "invisible.exe\"";
        reg(true, false, "Add to PATH", new[] { "Add to PATH" }, new[] { dir + " \"path\" \"%1\"" }, "Directory", "%SystemRoot%\\System32\\shell32.dll,-16773", "Middle");
        reg(true, false, "Add to PATH", new[] { "Add to PATH" }, new[] { dir + " \"path\" \"%w\"" }, "Directory\\Background", "%SystemRoot%\\System32\\shell32.dll,-16773", "Middle");

    }

    private void no_path_Click(object sender, EventArgs e)
    {
        try
        {
            reg(false, false, "Compact", null, null, "Directory", null, null);
            reg(false, false, "Compact", null, null, "Directory\\Background", null, null);
        }
        catch (Exception)
        {
            
        };
    }

    private void yes_batch_Click(object sender, EventArgs e)
    {
        RegistryKey reg = Registry.ClassesRoot.CreateSubKey(".bat\\ShellNew\\");
        reg.SetValue("NullFile", "1");
    }

    private void no_batch_Click(object sender, EventArgs e)
    {
        try
        {
            Registry.ClassesRoot.DeleteSubKey(".bat\\ShellNew\\");
        }
        catch (Exception){};
    }
}