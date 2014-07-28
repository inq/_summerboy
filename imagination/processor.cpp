// This is the main DLL file.



#include "stdafx.h"

#include "processor.h"
#include "..\open_surf\matcher.h"

using namespace System::Runtime::InteropServices;


namespace Imagination {

	Processor::Processor(String^ src, array<String^>^ targets) {
		String^ hello;
		array<char *>^ targets_ptr = gcnew array<char *>(targets->Length);
		int i = 0;
		for each(String^ target in targets) {
			targets_ptr[i] = (char *)Marshal::StringToHGlobalAnsi(target).ToPointer();
			i++;
		}
		hello = src + targets[0];
		pin_ptr<char *> ptr = &targets_ptr[0];
		Matcher *matcher = new Matcher((char *)Marshal::StringToHGlobalAnsi(src).ToPointer(), ptr, targets->Length);
		//mainStaticMatch();
	}

}