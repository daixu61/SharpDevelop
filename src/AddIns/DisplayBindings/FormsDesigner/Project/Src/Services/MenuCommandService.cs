﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Drawing;
using System.Windows.Forms;
using CommandID = System.ComponentModel.Design.CommandID;
using MenuCommand = System.ComponentModel.Design.MenuCommand;
using StandardCommands = System.ComponentModel.Design.StandardCommands;

namespace ICSharpCode.FormsDesigner.Services
{
	public class MenuCommandService : System.ComponentModel.Design.MenuCommandService
	{
		public readonly MenuCommandServiceProxy Proxy;
		
		ICommandProvider commandProvider;
		
		public MenuCommandService(ICommandProvider commandProvider, IServiceProvider serviceProvider) : base(serviceProvider)
		{
			Proxy = new MenuCommandServiceProxy(this);
			this.commandProvider = commandProvider;
			AddProxyCommand(delegate {
			                	IFormsDesigner fd = serviceProvider.GetService(typeof(IFormsDesigner)) as IFormsDesigner;
			                	if (fd != null)
			                		fd.ShowSourceCode();
			                }, StandardCommands.ViewCode);
			AddProxyCommand(delegate {
			                	IMessageService ms = serviceProvider.GetService(typeof(IMessageService)) as IMessageService;
			                	if (ms != null)
			                		ms.ShowPropertiesPad();
			                }, StandardCommands.PropertiesWindow);
		}

		public override void ShowContextMenu(CommandID menuID, int x, int y)
		{
			commandProvider.ShowContextMenu(menuID, x, y);
		}
		
		public void AddProxyCommand(EventHandler commandCallBack, CommandID commandID)
		{
			AddCommand(new MenuCommand(commandCallBack, commandID));
		}
	}
	
	public class MenuCommandServiceProxy : MarshalByRefObject, System.ComponentModel.Design.IMenuCommandService
	{
		MenuCommandService mcs;
		
		public MenuCommandServiceProxy(MenuCommandService mcs)
		{
			this.mcs = mcs;
		}
		
		public System.ComponentModel.Design.DesignerVerbCollection Verbs {
			get {
				return mcs.Verbs;
			}
		}
		
		public void AddCommand(MenuCommand command)
		{
			mcs.AddCommand(command);
		}
		
		public MenuCommand FindCommand(CommandID commandID)
		{
			return mcs.FindCommand(commandID);
		}
		
		public void ShowContextMenu(CommandID menuID, int x, int y)
		{
			mcs.ShowContextMenu(menuID, x, y);
		}
		
		public void AddVerb(System.ComponentModel.Design.DesignerVerb verb)
		{
			mcs.AddVerb(verb);
		}
		
		public bool GlobalInvoke(CommandID commandID)
		{
			return mcs.GlobalInvoke(commandID);
		}
		
		public void RemoveCommand(MenuCommand command)
		{
			mcs.RemoveCommand(command);
		}
		
		public void RemoveVerb(System.ComponentModel.Design.DesignerVerb verb)
		{
			mcs.RemoveVerb(verb);
		}
	}
}
