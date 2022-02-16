using System;
using System.Runtime.InteropServices;

public class JNI
{
	[DllImport("jni")]
	public static extern int GetVersion();

	[DllImport("jni")]
	public static extern IntPtr FindClass([MarshalAs(UnmanagedType.LPStr)] string name);

	[DllImport("jni")]
	public static extern int FromReflectedMethod(IntPtr method);

	[DllImport("jni")]
	public static extern int FromReflectedField(IntPtr field);

	[DllImport("jni")]
	public static extern IntPtr ToReflectedMethod(IntPtr cls, int methodID, int isStatic);

	[DllImport("jni")]
	public static extern IntPtr GetSuperclass(IntPtr clazz);

	[DllImport("jni")]
	public static extern int IsAssignableFrom(IntPtr clazz1, IntPtr clazz2);

	[DllImport("jni")]
	public static extern IntPtr ToReflectedField(IntPtr cls, int fieldID, int isStatic);

	[DllImport("jni")]
	public static extern int Throw(IntPtr obj);

	[DllImport("jni")]
	public static extern int ThrowNew(IntPtr clazz, [MarshalAs(UnmanagedType.LPStr)] string message);

	[DllImport("jni")]
	public static extern IntPtr ExceptionOccurred();

	[DllImport("jni")]
	public static extern void ExceptionDescribe();

	[DllImport("jni")]
	public static extern void ExceptionClear();

	[DllImport("jni")]
	public static extern void FatalError([MarshalAs(UnmanagedType.LPStr)] string msg);

	[DllImport("jni")]
	public static extern int PushLocalFrame(int capacity);

	[DllImport("jni")]
	public static extern IntPtr PopLocalFrame(IntPtr result);

	[DllImport("jni")]
	public static extern IntPtr NewGlobalRef(IntPtr obj);

	[DllImport("jni")]
	public static extern void DeleteGlobalRef(IntPtr globalRef);

	[DllImport("jni")]
	public static extern void DeleteLocalRef(IntPtr localRef);

	[DllImport("jni")]
	public static extern int IsSameObject(IntPtr ref1, IntPtr ref2);

	[DllImport("jni")]
	public static extern IntPtr NewLocalRef(IntPtr reference);

	[DllImport("jni")]
	public static extern int EnsureLocalCapacity(int capacity);

	[DllImport("jni")]
	public static extern IntPtr AllocObject(IntPtr clazz);

	[DllImport("jni")]
	public static extern IntPtr NewObject(IntPtr clazz, int methodID);

	[DllImport("jni")]
	public static extern IntPtr NewObject(IntPtr clazz, int methodID, IntPtr a);

	[DllImport("jni")]
	public static extern IntPtr NewObject(IntPtr clazz, int methodID, IntPtr a, IntPtr b);

	[DllImport("jni")]
	public static extern IntPtr NewObject(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c);

	[DllImport("jni")]
	public static extern IntPtr NewObject(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c, IntPtr d);

	[DllImport("jni")]
	public static extern IntPtr GetObjectClass(IntPtr obj);

	[DllImport("jni")]
	public static extern int IsInstanceOf(IntPtr obj, IntPtr clazz);

	[DllImport("jni")]
	public static extern int GetMethodID(IntPtr clazz, [MarshalAs(UnmanagedType.LPStr)] string name, [MarshalAs(UnmanagedType.LPStr)] string sig);

	[DllImport("jni")]
	public static extern void CallVoidMethod(IntPtr clazz, int methodID);

	[DllImport("jni")]
	public static extern void CallVoidMethod(IntPtr clazz, int methodID, IntPtr a);

	[DllImport("jni")]
	public static extern void CallVoidMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b);

	[DllImport("jni")]
	public static extern void CallVoidMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c);

	[DllImport("jni")]
	public static extern void CallVoidMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c, IntPtr d);

	[DllImport("jni")]
	public static extern IntPtr CallObjectMethod(IntPtr clazz, int methodID);

	[DllImport("jni")]
	public static extern IntPtr CallObjectMethod(IntPtr clazz, int methodID, IntPtr a);

	[DllImport("jni")]
	public static extern IntPtr CallObjectMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b);

	[DllImport("jni")]
	public static extern IntPtr CallObjectMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c);

	[DllImport("jni")]
	public static extern IntPtr CallObjectMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c, IntPtr d);

	[DllImport("jni")]
	public static extern int CallBooleanMethod(IntPtr clazz, int methodID);

	[DllImport("jni")]
	public static extern int CallBooleanMethod(IntPtr clazz, int methodID, IntPtr a);

	[DllImport("jni")]
	public static extern int CallBooleanMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b);

	[DllImport("jni")]
	public static extern int CallBooleanMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c);

	[DllImport("jni")]
	public static extern int CallBooleanMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c, IntPtr d);

	[DllImport("jni")]
	public static extern byte CallByteMethod(IntPtr clazz, int methodID);

	[DllImport("jni")]
	public static extern byte CallByteMethod(IntPtr clazz, int methodID, IntPtr a);

	[DllImport("jni")]
	public static extern byte CallByteMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b);

	[DllImport("jni")]
	public static extern byte CallByteMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c);

	[DllImport("jni")]
	public static extern byte CallByteMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c, IntPtr d);

	[DllImport("jni")]
	public static extern char CallCharMethod(IntPtr clazz, int methodID);

	[DllImport("jni")]
	public static extern char CallCharMethod(IntPtr clazz, int methodID, IntPtr a);

	[DllImport("jni")]
	public static extern char CallCharMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b);

	[DllImport("jni")]
	public static extern char CallCharMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c);

	[DllImport("jni")]
	public static extern char CallCharMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c, IntPtr d);

	[DllImport("jni")]
	public static extern short CallShortMethod(IntPtr clazz, int methodID);

	[DllImport("jni")]
	public static extern short CallShortMethod(IntPtr clazz, int methodID, IntPtr a);

	[DllImport("jni")]
	public static extern short CallShortMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b);

	[DllImport("jni")]
	public static extern short CallShortMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c);

	[DllImport("jni")]
	public static extern short CallShortMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c, IntPtr d);

	[DllImport("jni")]
	public static extern int CallIntMethod(IntPtr clazz, int methodID);

	[DllImport("jni")]
	public static extern int CallIntMethod(IntPtr clazz, int methodID, IntPtr a);

	[DllImport("jni")]
	public static extern int CallIntMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b);

	[DllImport("jni")]
	public static extern int CallIntMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c);

	[DllImport("jni")]
	public static extern int CallIntMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c, IntPtr d);

	[DllImport("jni")]
	public static extern long CallLongMethod(IntPtr clazz, int methodID);

	[DllImport("jni")]
	public static extern long CallLongMethod(IntPtr clazz, int methodID, IntPtr a);

	[DllImport("jni")]
	public static extern long CallLongMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b);

	[DllImport("jni")]
	public static extern long CallLongMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c);

	[DllImport("jni")]
	public static extern long CallLongMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c, IntPtr d);

	[DllImport("jni")]
	public static extern float CallFloatMethod(IntPtr clazz, int methodID);

	[DllImport("jni")]
	public static extern float CallFloatMethod(IntPtr clazz, int methodID, IntPtr a);

	[DllImport("jni")]
	public static extern float CallFloatMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b);

	[DllImport("jni")]
	public static extern float CallFloatMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c);

	[DllImport("jni")]
	public static extern float CallFloatMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c, IntPtr d);

	[DllImport("jni")]
	public static extern double CallDoubleMethod(IntPtr clazz, int methodID);

	[DllImport("jni")]
	public static extern double CallDoubleMethod(IntPtr clazz, int methodID, IntPtr a);

	[DllImport("jni")]
	public static extern double CallDoubleMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b);

	[DllImport("jni")]
	public static extern double CallDoubleMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c);

	[DllImport("jni")]
	public static extern double CallDoubleMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c, IntPtr d);

	[DllImport("jni")]
	public static extern void CallNonvirtualVoidMethod(IntPtr clazz, int methodID);

	[DllImport("jni")]
	public static extern void CallNonvirtualVoidMethod(IntPtr clazz, int methodID, IntPtr a);

	[DllImport("jni")]
	public static extern void CallNonvirtualVoidMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b);

	[DllImport("jni")]
	public static extern void CallNonvirtualVoidMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c);

	[DllImport("jni")]
	public static extern void CallNonvirtualVoidMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c, IntPtr d);

	[DllImport("jni")]
	public static extern IntPtr CallNonvirtualObjectMethod(IntPtr clazz, int methodID);

	[DllImport("jni")]
	public static extern IntPtr CallNonvirtualObjectMethod(IntPtr clazz, int methodID, IntPtr a);

	[DllImport("jni")]
	public static extern IntPtr CallNonvirtualObjectMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b);

	[DllImport("jni")]
	public static extern IntPtr CallNonvirtualObjectMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c);

	[DllImport("jni")]
	public static extern IntPtr CallNonvirtualObjectMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c, IntPtr d);

	[DllImport("jni")]
	public static extern int CallNonvirtualBooleanMethod(IntPtr clazz, int methodID);

	[DllImport("jni")]
	public static extern int CallNonvirtualBooleanMethod(IntPtr clazz, int methodID, IntPtr a);

	[DllImport("jni")]
	public static extern int CallNonvirtualBooleanMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b);

	[DllImport("jni")]
	public static extern int CallNonvirtualBooleanMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c);

	[DllImport("jni")]
	public static extern int CallNonvirtualBooleanMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c, IntPtr d);

	[DllImport("jni")]
	public static extern byte CallNonvirtualByteMethod(IntPtr clazz, int methodID);

	[DllImport("jni")]
	public static extern byte CallNonvirtualByteMethod(IntPtr clazz, int methodID, IntPtr a);

	[DllImport("jni")]
	public static extern byte CallNonvirtualByteMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b);

	[DllImport("jni")]
	public static extern byte CallNonvirtualByteMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c);

	[DllImport("jni")]
	public static extern byte CallNonvirtualByteMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c, IntPtr d);

	[DllImport("jni")]
	public static extern char CallNonvirtualCharMethod(IntPtr clazz, int methodID);

	[DllImport("jni")]
	public static extern char CallNonvirtualCharMethod(IntPtr clazz, int methodID, IntPtr a);

	[DllImport("jni")]
	public static extern char CallNonvirtualCharMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b);

	[DllImport("jni")]
	public static extern char CallNonvirtualCharMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c);

	[DllImport("jni")]
	public static extern char CallNonvirtualCharMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c, IntPtr d);

	[DllImport("jni")]
	public static extern short CallNonvirtualShortMethod(IntPtr clazz, int methodID);

	[DllImport("jni")]
	public static extern short CallNonvirtualShortMethod(IntPtr clazz, int methodID, IntPtr a);

	[DllImport("jni")]
	public static extern short CallNonvirtualShortMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b);

	[DllImport("jni")]
	public static extern short CallNonvirtualShortMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c);

	[DllImport("jni")]
	public static extern short CallNonvirtualShortMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c, IntPtr d);

	[DllImport("jni")]
	public static extern int CallNonvirtualIntMethod(IntPtr clazz, int methodID);

	[DllImport("jni")]
	public static extern int CallNonvirtualIntMethod(IntPtr clazz, int methodID, IntPtr a);

	[DllImport("jni")]
	public static extern int CallNonvirtualIntMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b);

	[DllImport("jni")]
	public static extern int CallNonvirtualIntMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c);

	[DllImport("jni")]
	public static extern int CallNonvirtualIntMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c, IntPtr d);

	[DllImport("jni")]
	public static extern long CallNonvirtualLongMethod(IntPtr clazz, int methodID);

	[DllImport("jni")]
	public static extern long CallNonvirtualLongMethod(IntPtr clazz, int methodID, IntPtr a);

	[DllImport("jni")]
	public static extern long CallNonvirtualLongMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b);

	[DllImport("jni")]
	public static extern long CallNonvirtualLongMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c);

	[DllImport("jni")]
	public static extern long CallNonvirtualLongMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c, IntPtr d);

	[DllImport("jni")]
	public static extern float CallNonvirtualFloatMethod(IntPtr clazz, int methodID);

	[DllImport("jni")]
	public static extern float CallNonvirtualFloatMethod(IntPtr clazz, int methodID, IntPtr a);

	[DllImport("jni")]
	public static extern float CallNonvirtualFloatMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b);

	[DllImport("jni")]
	public static extern float CallNonvirtualFloatMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c);

	[DllImport("jni")]
	public static extern float CallNonvirtualFloatMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c, IntPtr d);

	[DllImport("jni")]
	public static extern double CallNonvirtualDoubleMethod(IntPtr clazz, int methodID);

	[DllImport("jni")]
	public static extern double CallNonvirtualDoubleMethod(IntPtr clazz, int methodID, IntPtr a);

	[DllImport("jni")]
	public static extern double CallNonvirtualDoubleMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b);

	[DllImport("jni")]
	public static extern double CallNonvirtualDoubleMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c);

	[DllImport("jni")]
	public static extern double CallNonvirtualDoubleMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c, IntPtr d);

	[DllImport("jni")]
	public static extern int GetFieldID(IntPtr clazz, [MarshalAs(UnmanagedType.LPStr)] string name, [MarshalAs(UnmanagedType.LPStr)] string sig);

	[DllImport("jni")]
	public static extern IntPtr GetObjectField(IntPtr obj, int fieldID);

	[DllImport("jni")]
	public static extern int GetBooleanField(IntPtr obj, int fieldID);

	[DllImport("jni")]
	public static extern byte GetByteField(IntPtr obj, int fieldID);

	[DllImport("jni")]
	public static extern char GetCharField(IntPtr obj, int fieldID);

	[DllImport("jni")]
	public static extern short GetShortField(IntPtr obj, int fieldID);

	[DllImport("jni")]
	public static extern int GetIntField(IntPtr obj, int fieldID);

	[DllImport("jni")]
	public static extern long GetLongField(IntPtr obj, int fieldID);

	[DllImport("jni")]
	public static extern float GetFloatField(IntPtr obj, int fieldID);

	[DllImport("jni")]
	public static extern double GetDoubleField(IntPtr obj, int fieldID);

	[DllImport("jni")]
	public static extern void SetObjectField(IntPtr obj, int fieldID, IntPtr value);

	[DllImport("jni")]
	public static extern void SetBooleanField(IntPtr obj, int fieldID, int value);

	[DllImport("jni")]
	public static extern void SetByteField(IntPtr obj, int fieldID, byte value);

	[DllImport("jni")]
	public static extern void SetCharField(IntPtr obj, int fieldID, char value);

	[DllImport("jni")]
	public static extern void SetShortField(IntPtr obj, int fieldID, short value);

	[DllImport("jni")]
	public static extern void SetIntField(IntPtr obj, int fieldID, int value);

	[DllImport("jni")]
	public static extern void SetLongField(IntPtr obj, int fieldID, long value);

	[DllImport("jni")]
	public static extern void SetFloatField(IntPtr obj, int fieldID, float value);

	[DllImport("jni")]
	public static extern void SetDoubleField(IntPtr obj, int fieldID, double value);

	[DllImport("jni")]
	public static extern int GetStaticMethodID(IntPtr clazz, [MarshalAs(UnmanagedType.LPStr)] string name, [MarshalAs(UnmanagedType.LPStr)] string sig);

	[DllImport("jni")]
	public static extern void CallStaticVoidMethod(IntPtr clazz, int methodID);

	[DllImport("jni")]
	public static extern void CallStaticVoidMethod(IntPtr clazz, int methodID, IntPtr a);

	[DllImport("jni")]
	public static extern void CallStaticVoidMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b);

	[DllImport("jni")]
	public static extern void CallStaticVoidMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c);

	[DllImport("jni")]
	public static extern void CallStaticVoidMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c, IntPtr d);

	[DllImport("jni")]
	public static extern IntPtr CallStaticObjectMethod(IntPtr clazz, int methodID);

	[DllImport("jni")]
	public static extern IntPtr CallStaticObjectMethod(IntPtr clazz, int methodID, IntPtr a);

	[DllImport("jni")]
	public static extern IntPtr CallStaticObjectMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b);

	[DllImport("jni")]
	public static extern IntPtr CallStaticObjectMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c);

	[DllImport("jni")]
	public static extern IntPtr CallStaticObjectMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c, IntPtr d);

	[DllImport("jni")]
	public static extern int CallStaticBooleanMethod(IntPtr clazz, int methodID);

	[DllImport("jni")]
	public static extern int CallStaticBooleanMethod(IntPtr clazz, int methodID, IntPtr a);

	[DllImport("jni")]
	public static extern int CallStaticBooleanMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b);

	[DllImport("jni")]
	public static extern int CallStaticBooleanMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c);

	[DllImport("jni")]
	public static extern int CallStaticBooleanMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c, IntPtr d);

	[DllImport("jni")]
	public static extern byte CallStaticByteMethod(IntPtr clazz, int methodID);

	[DllImport("jni")]
	public static extern byte CallStaticByteMethod(IntPtr clazz, int methodID, IntPtr a);

	[DllImport("jni")]
	public static extern byte CallStaticByteMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b);

	[DllImport("jni")]
	public static extern byte CallStaticByteMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c);

	[DllImport("jni")]
	public static extern byte CallStaticByteMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c, IntPtr d);

	[DllImport("jni")]
	public static extern char CallStaticCharMethod(IntPtr clazz, int methodID);

	[DllImport("jni")]
	public static extern char CallStaticCharMethod(IntPtr clazz, int methodID, IntPtr a);

	[DllImport("jni")]
	public static extern char CallStaticCharMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b);

	[DllImport("jni")]
	public static extern char CallStaticCharMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c);

	[DllImport("jni")]
	public static extern char CallStaticCharMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c, IntPtr d);

	[DllImport("jni")]
	public static extern short CallStaticShortMethod(IntPtr clazz, int methodID);

	[DllImport("jni")]
	public static extern short CallStaticShortMethod(IntPtr clazz, int methodID, IntPtr a);

	[DllImport("jni")]
	public static extern short CallStaticShortMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b);

	[DllImport("jni")]
	public static extern short CallStaticShortMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c);

	[DllImport("jni")]
	public static extern short CallStaticShortMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c, IntPtr d);

	[DllImport("jni")]
	public static extern int CallStaticIntMethod(IntPtr clazz, int methodID);

	[DllImport("jni")]
	public static extern int CallStaticIntMethod(IntPtr clazz, int methodID, IntPtr a);

	[DllImport("jni")]
	public static extern int CallStaticIntMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b);

	[DllImport("jni")]
	public static extern int CallStaticIntMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c);

	[DllImport("jni")]
	public static extern int CallStaticIntMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c, IntPtr d);

	[DllImport("jni")]
	public static extern long CallStaticLongMethod(IntPtr clazz, int methodID);

	[DllImport("jni")]
	public static extern long CallStaticLongMethod(IntPtr clazz, int methodID, IntPtr a);

	[DllImport("jni")]
	public static extern long CallStaticLongMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b);

	[DllImport("jni")]
	public static extern long CallStaticLongMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c);

	[DllImport("jni")]
	public static extern long CallStaticLongMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c, IntPtr d);

	[DllImport("jni")]
	public static extern float CallStaticFloatMethod(IntPtr clazz, int methodID);

	[DllImport("jni")]
	public static extern float CallStaticFloatMethod(IntPtr clazz, int methodID, IntPtr a);

	[DllImport("jni")]
	public static extern float CallStaticFloatMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b);

	[DllImport("jni")]
	public static extern float CallStaticFloatMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c);

	[DllImport("jni")]
	public static extern float CallStaticFloatMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c, IntPtr d);

	[DllImport("jni")]
	public static extern double CallStaticDoubleMethod(IntPtr clazz, int methodID);

	[DllImport("jni")]
	public static extern double CallStaticDoubleMethod(IntPtr clazz, int methodID, IntPtr a);

	[DllImport("jni")]
	public static extern double CallStaticDoubleMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b);

	[DllImport("jni")]
	public static extern double CallStaticDoubleMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c);

	[DllImport("jni")]
	public static extern double CallStaticDoubleMethod(IntPtr clazz, int methodID, IntPtr a, IntPtr b, IntPtr c, IntPtr d);

	[DllImport("jni")]
	public static extern int GetStaticFieldID(IntPtr clazz, [MarshalAs(UnmanagedType.LPStr)] string name, [MarshalAs(UnmanagedType.LPStr)] string sig);

	[DllImport("jni")]
	public static extern IntPtr GetStaticObjectField(IntPtr clazz, int fieldID);

	[DllImport("jni")]
	public static extern int GetStaticBooleanField(IntPtr clazz, int fieldID);

	[DllImport("jni")]
	public static extern byte GetStaticByteField(IntPtr clazz, int fieldID);

	[DllImport("jni")]
	public static extern char GetStaticCharField(IntPtr clazz, int fieldID);

	[DllImport("jni")]
	public static extern short GetStaticShortField(IntPtr clazz, int fieldID);

	[DllImport("jni")]
	public static extern int GetStaticIntField(IntPtr clazz, int fieldID);

	[DllImport("jni")]
	public static extern long GetStaticLongField(IntPtr clazz, int fieldID);

	[DllImport("jni")]
	public static extern float GetStaticFloatField(IntPtr clazz, int fieldID);

	[DllImport("jni")]
	public static extern double GetStaticDoubleField(IntPtr clazz, int fieldID);

	[DllImport("jni")]
	public static extern void SetStaticObjectField(IntPtr clazz, int fieldID, IntPtr value);

	[DllImport("jni")]
	public static extern void SetStaticBooleanField(IntPtr clazz, int fieldID, int value);

	[DllImport("jni")]
	public static extern void SetStaticByteField(IntPtr clazz, int fieldID, byte value);

	[DllImport("jni")]
	public static extern void SetStaticCharField(IntPtr clazz, int fieldID, char value);

	[DllImport("jni")]
	public static extern void SetStaticShortField(IntPtr clazz, int fieldID, short value);

	[DllImport("jni")]
	public static extern void SetStaticIntField(IntPtr clazz, int fieldID, int value);

	[DllImport("jni")]
	public static extern void SetStaticLongField(IntPtr clazz, int fieldID, long value);

	[DllImport("jni")]
	public static extern void SetStaticFloatField(IntPtr clazz, int fieldID, float value);

	[DllImport("jni")]
	public static extern void SetStaticDoubleField(IntPtr clazz, int fieldID, double value);

	[DllImport("jni")]
	public static extern IntPtr NewString([MarshalAs(UnmanagedType.LPStr)] string unicodeChars, int len);

	[DllImport("jni")]
	public static extern int GetStringLength(IntPtr IntPtr);

	[DllImport("jni")]
	public static extern IntPtr GetStringChars(IntPtr jstring, int setToZero);

	[DllImport("jni")]
	public static extern void ReleaseStringChars(IntPtr jstring, IntPtr chars);

	[DllImport("jni")]
	public static extern IntPtr NewStringUTF([MarshalAs(UnmanagedType.LPStr)] string bytes);

	[DllImport("jni")]
	public static extern int GetStringUTFLength(IntPtr jstring);

	[DllImport("jni")]
	public static extern IntPtr GetStringUTFChars(IntPtr jstring, int setToZero);

	[DllImport("jni")]
	public static extern void ReleaseStringUTFChars(IntPtr jstring, IntPtr utf);

	[DllImport("jni")]
	public static extern int GetArrayLength(IntPtr array);

	[DllImport("jni")]
	public static extern IntPtr NewObjectArray(int length, IntPtr elementClass, IntPtr initialElement);

	[DllImport("jni")]
	public static extern IntPtr GetObjectArrayElement(IntPtr array, int index);

	[DllImport("jni")]
	public static extern void SetObjectArrayElement(IntPtr array, int index, IntPtr value);

	[DllImport("jni")]
	public static extern IntPtr NewBooleanArray(int length);

	[DllImport("jni")]
	public static extern IntPtr NewByteArray(int length);

	[DllImport("jni")]
	public static extern IntPtr NewCharArray(int length);

	[DllImport("jni")]
	public static extern IntPtr NewShortArray(int length);

	[DllImport("jni")]
	public static extern IntPtr NewIntArray(int length);

	[DllImport("jni")]
	public static extern IntPtr NewLongArray(int length);

	[DllImport("jni")]
	public static extern IntPtr NewFloatArray(int length);

	[DllImport("jni")]
	public static extern IntPtr NewDoubleArray(int length);

	[DllImport("jni")]
	public static extern int MonitorEnter(IntPtr obj);

	[DllImport("jni")]
	public static extern int MonitorExit(IntPtr obj);

	[DllImport("jni")]
	public static extern IntPtr GetJavaVM();

	[DllImport("jni")]
	public static extern string GetStringCritical(IntPtr IntPtr);

	[DllImport("jni")]
	public static extern IntPtr NewWeakGlobalRef(IntPtr obj);

	[DllImport("jni")]
	public static extern void DeleteWeakGlobalRef(IntPtr obj);

	[DllImport("jni")]
	public static extern int ExceptionCheck();

	[DllImport("jni")]
	public static extern long GetDirectBufferCapacity(IntPtr buf);
}
