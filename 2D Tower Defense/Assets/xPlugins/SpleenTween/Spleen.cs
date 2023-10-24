using System;
using SpleenTween.Extensions;
using UnityEngine;

namespace SpleenTween
{
    /// <summary>
    /// Call the functions in this to start a tween
    /// </summary>
    public static class Spleen
    {
        public static void AddTween(Tween tween) => SpleenTweenManager.Instance.AddTween(tween);
        public static void StopTween(Tween tween) => SpleenTweenManager.Instance.RemoveTween(tween);
        public static void StopAllTweens() => SpleenTweenManager.Instance.RemoveAllTweens();
        public static void StopAllTweens(GameObject target) => SpleenTweenManager.Instance.RemoveAllTweensWithIdentifier(target);

        static Tween CreateTween<T>(T from, T to, float duration, Ease easing, Action<T> onUpdate)
        {
            Tween tween = new TweenInstance<T>(from, to, duration, easing, onUpdate);
            AddTween(tween);
            return tween;
        }

        static Tween CreateTargetTween<T,K>(K target, GameObject identifier, T from, T to, float duration, Ease easing, Action<T> onUpdate)
        {
            Tween tween = new TweenInstance<T>(from, to, duration, easing, onUpdate, identifier, () => identifier == null || target == null || target.Equals(null));
            AddTween(tween);
            return tween;
        }

        static Tween CreateRelativeTargetTween<T, K>(K target, GameObject identifier, T increment, float duration, Ease easing, Func<T> currentVal, Action<T, T> onUpdate)
        {
            T current = currentVal.Invoke();
            T from = current;
            T to = SpleenExt.AddGenerics(current, increment);

            Tween tween = new TweenInstance<T>(from, to, duration, easing, (val) =>
            {
                onUpdate.Invoke(val, current);
                current = val;
            }, identifier, () => target == null || target.Equals(null) || identifier == null);

            tween.OnStart(() =>
            {
                if (Looping.IsLoopWeird(tween.LoopType)) return;

                current = currentVal.Invoke();
                from = current;
                to = SpleenExt.AddGenerics(current, increment);
                tween.From = from;
                tween.To = to;
            });

            AddTween(tween);
            return tween;
        }

        #region User Facing Functions
        public static Tween Value(float from, float to, float duration, Ease easing, Action<float> onUpdate) => 
            CreateTween(from, to, duration, easing, onUpdate);
        public static Tween Value3(Vector3 from, Vector3 to, float duration, Ease easing, Action<Vector3> onUpdate) => 
            CreateTween(from, to, duration, easing, onUpdate);



        public static Tween Pos(Transform target, Vector3 from, Vector3 to, float duration, Ease easing) => 
            CreateTargetTween(target, target.gameObject, from, to, duration, easing, val => target.position = val);
        public static Tween PosX(Transform target, float from, float to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, from, to, duration, easing, val => SpleenExt.SetAxisPos(Axis.x, target, val));
        public static Tween PosY(Transform target, float from, float to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, from, to, duration, easing, val => SpleenExt.SetAxisPos(Axis.y, target, val));
        public static Tween PosZ(Transform target, float from, float to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, from, to, duration, easing, val => SpleenExt.SetAxisPos(Axis.z, target, val));

        public static Tween Pos(Transform target, Vector3 to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, target.position, to, duration, easing, val => target.position = val);
        public static Tween PosX(Transform target, float to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, target.position.x, to, duration, easing, val => SpleenExt.SetAxisPos(Axis.x, target, val));
        public static Tween PosY(Transform target, float to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, target.position.y, to, duration, easing, val => SpleenExt.SetAxisPos(Axis.y, target, val));
        public static Tween PosZ(Transform target, float to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, target.position.z, to, duration, easing, val => SpleenExt.SetAxisPos(Axis.z, target, val));

        public static Tween AddPos(Transform target, Vector3 increment, float duration, Ease easing) =>
            CreateRelativeTargetTween(target, target.gameObject, increment, duration, easing, () => target.position, (val, from) => 
            target.position += val - from);
        public static Tween AddPosX(Transform target, float increment, float duration, Ease easing) =>
            CreateRelativeTargetTween(target, target.gameObject, increment, duration, easing, () => target.position.x, (val, from) => 
            SpleenExt.AddAxisPos(Axis.x, target, val - from));
        public static Tween AddPosY(Transform target, float increment, float duration, Ease easing) =>
            CreateRelativeTargetTween(target, target.gameObject, increment, duration, easing, () => target.position.y, (val, from) =>
            SpleenExt.AddAxisPos(Axis.y, target, val - from));

        public static Tween AddPosZ(Transform target, float increment, float duration, Ease easing) =>
            CreateRelativeTargetTween(target, target.gameObject, increment, duration, easing, () => target.position.z, (val, from) =>
            SpleenExt.AddAxisPos(Axis.z, target, val - from));



        public static Tween Scale(Transform target, Vector3 from, Vector3 to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, from, to, duration, easing, val => target.localScale = val);
        public static Tween ScaleX(Transform target, float from, float to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, from, to, duration, easing, val => SpleenExt.SetAxisScale(Axis.x, target, val));
        public static Tween ScaleY(Transform target, float from, float to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, from, to, duration, easing, val => SpleenExt.SetAxisScale(Axis.y, target, val));
        public static Tween ScaleZ(Transform target, float from, float to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, from, to, duration, easing, val => SpleenExt.SetAxisScale(Axis.z, target, val));

        public static Tween Scale(Transform target, Vector3 to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, target.localScale, to, duration, easing, val => target.localScale = val);
        public static Tween ScaleX(Transform target, float to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, target.localScale.x, to, duration, easing, val => SpleenExt.SetAxisScale(Axis.x, target, val));
        public static Tween ScaleY(Transform target, float to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, target.localScale.y, to, duration, easing, val => SpleenExt.SetAxisScale(Axis.y, target, val));
        public static Tween ScaleZ(Transform target, float to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, target.localScale.z, to, duration, easing, val => SpleenExt.SetAxisScale(Axis.z, target, val));

        public static Tween AddScale(Transform target, Vector3 increment, float duration, Ease easing) =>
            CreateRelativeTargetTween(target, target.gameObject, increment, duration, easing, () => target.localScale, (val, from) => 
            target.localScale += val - from);
        public static Tween AddScaleX(Transform target, float increment, float duration, Ease easing) =>
            CreateRelativeTargetTween(target, target.gameObject, increment, duration, easing, () => target.localScale.x, (val, from) =>
            SpleenExt.AddAxisScale(Axis.x, target, val - from));
        public static Tween AddScaleY(Transform target, float increment, float duration, Ease easing) =>
            CreateRelativeTargetTween(target, target.gameObject, increment, duration, easing, () => target.localScale.y, (val, from) =>
            SpleenExt.AddAxisScale(Axis.y, target, val - from));

        public static Tween AddScaleZ(Transform target, float increment, float duration, Ease easing) =>
            CreateRelativeTargetTween(target, target.gameObject, increment, duration, easing, () => target.localScale.z, (val, from) =>
            SpleenExt.AddAxisScale(Axis.z, target, val - from));



        public static Tween Rot(Transform target, Vector3 from, Vector3 to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, from, to, duration, easing, val => target.eulerAngles = val);
        public static Tween RotX(Transform target, float from, float to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, from, to, duration, easing, val => SpleenExt.SetAxisRot(Axis.x, target, val));
        public static Tween RotY(Transform target, float from, float to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, from, to, duration, easing, val => SpleenExt.SetAxisRot(Axis.y, target, val));
        public static Tween RotZ(Transform target, float from, float to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, from, to, duration, easing, val => SpleenExt.SetAxisRot(Axis.z, target, val));

        public static Tween Rot(Transform target, Vector3 to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, target.eulerAngles, to, duration, easing, val => target.eulerAngles = val);
        public static Tween RotX(Transform target, float to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, target.eulerAngles.x, to, duration, easing, val => SpleenExt.SetAxisRot(Axis.x, target, val));
        public static Tween RotY(Transform target, float to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, target.eulerAngles.y, to, duration, easing, val => SpleenExt.SetAxisRot(Axis.y, target, val));
        public static Tween RotZ(Transform target, float to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, target.eulerAngles.z, to, duration, easing, val => SpleenExt.SetAxisRot(Axis.z, target, val));

        public static Tween AddRot(Transform target, Vector3 increment, float duration, Ease easing) =>
            CreateRelativeTargetTween(target, target.gameObject, increment, duration, easing, () => target.eulerAngles, (val, from) => 
            target.eulerAngles += val - from);
        public static Tween AddRotX(Transform target, float increment, float duration, Ease easing) =>
            CreateRelativeTargetTween(target, target.gameObject, increment, duration, easing, () => target.eulerAngles.x, (val, from) =>
            SpleenExt.AddAxisRot(Axis.x, target, val - from));
        public static Tween AddRotY(Transform target, float increment, float duration, Ease easing) =>
            CreateRelativeTargetTween(target, target.gameObject, increment, duration, easing, () => target.eulerAngles.y, (val, from) =>
            SpleenExt.AddAxisRot(Axis.y, target, val - from));

        public static Tween AddRotZ(Transform target, float increment, float duration, Ease easing) =>
            CreateRelativeTargetTween(target, target.gameObject, increment, duration, easing, () => target.eulerAngles.z, (val, from) =>
            SpleenExt.AddAxisRot(Axis.z, target, val - from));



        public static Tween Vol(AudioSource target, float from, float to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, from, to, duration, easing, val => target.volume = val);

        public static Tween Vol(AudioSource target, float to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, target.volume, to, duration, easing, val => target.volume = val);

        public static Tween AddVol(AudioSource target, float increment, float duration, Ease easing) =>
            CreateRelativeTargetTween(target, target.gameObject, increment, duration, easing, () => target.volume, (val, from) =>
            target.volume += val - from);



        public static Tween SRColor(SpriteRenderer target, Color from, Color to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, from, to, duration, easing, val => target.color = val);

        public static Tween SRColor(SpriteRenderer target, Color to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, target.color, to, duration, easing, val => target.color = val);

        public static Tween AddSRColor(SpriteRenderer target, Color increment, float duration, Ease easing) =>
            CreateRelativeTargetTween(target, target.gameObject, increment, duration, easing, () => target.color, (val, from) =>
            target.color += val - from);

        #endregion
    }
}
