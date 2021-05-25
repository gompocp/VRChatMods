using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace BetterVRCKeyboard
{

	public static class DOTweenWrapper
	{

		public static void Punch(Func<Vector3> getter, Action<Vector3> setter, Vector3 direction, float duration, int vibrato = 0, float elasticity = 0f) => 
			DOTween.Punch(ConvertFuncToGetter(getter), ConvertActionToSetter(setter), direction, duration, vibrato, elasticity);

		public static void Shake(Func<Vector3> getter, Action<Vector3> setter, float duration, Vector3 strength, int vibrato, float randomness, bool fadeOut = true) => 
			DOTween.Shake(ConvertFuncToGetter(getter), ConvertActionToSetter(setter), duration, strength, vibrato, randomness, fadeOut);

		public static void Shake(Func<Vector3> getter, Action<Vector3> setter, float duration, float strength, int vibrato, float randomness, bool ignoreZAxis = true, bool fadeOut = true) =>
			DOTween.Shake(ConvertFuncToGetter(getter), ConvertActionToSetter(setter), duration, strength, vibrato, randomness, ignoreZAxis, fadeOut);

		public static void Shake(Func<Vector3> getter, Action<Vector3> setter, float duration, Vector3 strength, int vibrato, float randomness, bool ignoreZAxis, bool vectorBased, bool fadeOut) =>
			DOTween.Shake(ConvertFuncToGetter(getter), ConvertActionToSetter(setter), duration, strength, vibrato, randomness, ignoreZAxis, vectorBased, fadeOut);
		
		public static TweenerCore<float, float, FloatOptions> To(Func<float> getter, Action<float> setter, float endValue, float duration) =>
			DOTween.To(ConvertFuncToGetter(getter), ConvertActionToSetter(setter), endValue, duration);

		public static TweenerCore<int, int, NoOptions> To(Func<int> getter, Action<int> setter, int endValue, float duration) =>
			DOTween.To(ConvertFuncToGetter(getter), ConvertActionToSetter(setter), endValue, duration);
		
		public static TweenerCore<string, string, StringOptions> To(Func<string> getter, Action<string> setter, string endValue, float duration) =>
			DOTween.To(ConvertFuncToGetter(getter), ConvertActionToSetter(setter), endValue, duration);

		public static TweenerCore<Vector2, Vector2, VectorOptions> To(Func<Vector2> getter, Action<Vector2> setter, Vector2 endValue, float duration) =>
			DOTween.To(ConvertFuncToGetter(getter), ConvertActionToSetter(setter), endValue, duration);

		public static TweenerCore<Quaternion, Vector3, QuaternionOptions> To(Func<Quaternion> getter, Action<Quaternion> setter, Vector3 endValue, float duration) =>
			DOTween.To(ConvertFuncToGetter(getter), ConvertActionToSetter(setter), endValue, duration);

		public static TweenerCore<Color, Color, ColorOptions> To(Func<Color> getter, Action<Color> setter, Color endValue, float duration) =>
			DOTween.To(ConvertFuncToGetter(getter), ConvertActionToSetter(setter), endValue, duration);

		public static TweenerCore<Vector3, Vector3, VectorOptions> To(Func<Vector3> getter, Action<Vector3> setter, Vector3 endValue, float duration) =>
			DOTween.To(ConvertFuncToGetter(getter), ConvertActionToSetter(setter), endValue, duration);

		public static TweenerCore<Color, Color, ColorOptions> ToAlpha(Func<Color> getter, Action<Color> setter, float endValue, float duration) =>
			DOTween.ToAlpha(ConvertFuncToGetter(getter), ConvertActionToSetter(setter), endValue, duration);

		private static DOGetter<T> ConvertFuncToGetter<T>(Func<T> func) =>
			DelegateSupport.ConvertDelegate<DOGetter<T>>(func);

		private static DOSetter<T> ConvertActionToSetter<T>(Action<T> action) => 
			DelegateSupport.ConvertDelegate<DOSetter<T>>(action);

		public static TweenerCore<Vector3, Vector3, VectorOptions> DOScale(this Transform target, Vector3 endValue, float duration) =>
			ShortcutExtensions.DOScale(target, endValue, duration);

		public static TweenerCore<Quaternion, Quaternion, NoOptions> DORotateQuaternion(this Transform target, Quaternion endValue, float duration) => 
			ShortcutExtensions.DORotateQuaternion(target, endValue, duration);
		
		public static Tweener DOPunchScale(Transform target, Vector3 punch, float duration, int vibrato = 0, float elasticity = 0f) =>
			ShortcutExtensions.DOPunchScale(target, punch, duration, vibrato, elasticity);
		
		
		public static TweenerCore<Vector3, Path, PathOptions> DOPath(Transform target, Path path, float duration, PathMode pathMode = PathMode.Ignore) => 
			ShortcutExtensions.DOPath(target, path, duration, pathMode);

		public static TweenerCore<Vector3, Vector3, VectorOptions> DOMove(Transform target, Vector3 endValue, float duration, bool snapping = false) =>
			ShortcutExtensions.DOMove(target, endValue, duration, snapping);

		public static TweenerCore<Vector3, Path, PathOptions> DOLocalPath(Transform target, Path path, float duration, PathMode pathMode = PathMode.Ignore) => 
			ShortcutExtensions.DOLocalPath(target, path, duration, pathMode);
		

		public static TweenerCore<Color, Color, ColorOptions> DOColor(Material target, Color endValue, string property, float duration) => 
			ShortcutExtensions.DOColor(target, endValue, property, duration);

		public static TweenerCore<Color, Color, ColorOptions> DOColor(Material target, Color endValue, float duration) => 
			ShortcutExtensions.DOColor(target, endValue, duration);
		
		public static void AddAction(this TweenCallback callback, Action action) => 
			callback = DelegateSupport.ConvertDelegate<TweenCallback>(action); 
	}
}