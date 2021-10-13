mergeInto(LibraryManager.library, {
	SaveGameData: function(stringEmailData, stringCueData) {
		ReactUnityWebGL.SaveGameData(Pointer_stringify(stringEmailData),Pointer_stringify(stringCueData));
	},
});