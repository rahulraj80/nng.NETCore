using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace nng.Pinvoke
{
    using static Globals;
    
    public sealed class Socket
    {
        // [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void nng_fini();

        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 nng_close(nng_socket socket);

        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 nng_socket_id(nng_socket socket);

        // [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        // public static extern Int32 nng_closeall();

        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 nng_setopt(nng_socket socket, string name, IntPtr data, UIntPtr size);

        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 nng_setopt_bool(nng_socket socket, bool value);

        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 nng_setopt_int(nng_socket socket, Int32 value);

        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 nng_setopt_ms(nng_socket socket, nng_duration value);

        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 nng_setopt_size(nng_socket socket, UIntPtr value);

        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 nng_setopt_uint64(nng_socket socket, UInt64 value);

        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 nng_setopt_string(nng_socket socket, string value);

        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 nng_setopt_ptr(nng_socket socket, IntPtr value);

        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 nng_getopt(nng_socket socket, string name, out IntPtr data, out UIntPtr size);

        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 nng_getopt_bool(nng_socket socket, out bool value);

        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 nng_getopt_int(nng_socket socket, out Int32 value);

        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 nng_getopt_ms(nng_socket socket, out nng_duration value);

        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 nng_getopt_size(nng_socket socket, out UIntPtr value);

        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 nng_getopt_uint64(nng_socket socket, out UInt64 value);

        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 nng_getopt_ptr(nng_socket socket, out IntPtr value);



        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 nng_send(nng_socket socket, IntPtr data, UIntPtr size, Int32 flags);

        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 nng_recv(nng_socket socket, out IntPtr data, out UIntPtr size, Int32 flags);

        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 nng_sendmsg(nng_socket socket, nng_msg message, Int32 flags);

        [DllImport(NngDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 nng_recvmsg(nng_socket socket, out nng_msg message, Int32 flags);
    }
}