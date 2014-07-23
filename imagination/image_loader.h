#pragma once
#include "..\imagination_native\n_image_loader.h"

using namespace System;

namespace Imagination {
	public ref class ImageLoader {
	public:
		ImageLoader();
		void set724(int);
		int get724();

	protected:
		NImageLoader *n_image_loader;
	};
}