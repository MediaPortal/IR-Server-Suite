//////////////////////////////////////////////////
// Created by Ivan Latunov - IvanWeb.com        //
//----------------------------------------------//
// This program is free software. You can       //
// redistribute it and/or modify it as you wish //
//////////////////////////////////////////////////


using System;

namespace AppModule.InterProcessComm {
	#region Comments
	/// <summary>
	/// 
	/// </summary>
	#endregion
	public interface IInterProcessConnection : IDisposable {
		#region Comments
		/// <summary>
		/// 
		/// </summary>
		#endregion
		int NativeHandle{get;}
		#region Comments
		/// <summary>
		/// 
		/// </summary>
		#endregion
		void Connect();
		#region Comments
		/// <summary>
		/// 
		/// </summary>
		#endregion
		void Close();
		#region Comments
		/// <summary>
		/// 
		/// </summary>
		/// <returns>.</returns>
		#endregion
		string Read();
		#region Comments
		/// <summary>
		/// 
		/// </summary>
		/// <returns>.</returns>
		#endregion
		byte[] ReadBytes();
		#region Comments
		/// <summary>
		/// 
		/// </summary>
		/// <param name="text">.</param>
		#endregion
		void Write(string text);
		#region Comments
		/// <summary>
		/// 
		/// </summary>
		/// <param name="bytes">.</param>
		#endregion
		void WriteBytes(byte[] bytes);
		#region Comments
		/// <summary>
		/// 
		/// </summary>
		/// <returns>.</returns>
		#endregion
		InterProcessConnectionState GetState();
	}
}