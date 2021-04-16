using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace TinyTools.Juicy
{
    public enum ShaderKeyword
    {
        None,
        AlphaTestOn,
        AlphaBlendOn,
        AlphaPremultiplyOn,
        Emission,
        MetallicGlossMap,
        SpecGlossMap
    }   
    
    [Feedback("Renderer/Shader")][AddComponentMenu("")]
    public class JuicyFeedbackShader : JuicyFeedbackBase
    {
        public static string Float = "Float";
        public static string Vector = "Vector";
        public static string Color = "Color";
        public static string Range = "Range";

        private static readonly Dictionary<ShaderKeyword, string> Keywords = new Dictionary<ShaderKeyword, string> {
            {ShaderKeyword.None, null},
            {ShaderKeyword.AlphaTestOn, "_ALPHATEST_ON"},
            {ShaderKeyword.AlphaBlendOn, "_ALPHABLEND_ON"},
            {ShaderKeyword.AlphaPremultiplyOn, "_ALPHAPREMULTIPLY_ON"},
            {ShaderKeyword.Emission, "_EMISSION"},
            {ShaderKeyword.MetallicGlossMap, "_METALLICGLOSSMAP"},
            {ShaderKeyword.SpecGlossMap, "_SPECGLOSSMAP"},
        };

        [SerializeField, Timing] private Timing timing = new Timing();
        [SerializeField] private RendererTarget target = new RendererTarget();
        
        [SerializeField] private string propertyName = "";
#pragma warning disable 0414  
        [SerializeField] private int selected = 0;
#pragma warning restore 0414  
        [SerializeField] private string propertyType = "";
        
        [SerializeField] private FloatFromToValue floatValue = new FloatFromToValue();
        [SerializeField] private Vector4FromToValue vectorValue = new Vector4FromToValue();
        [SerializeField] private ColorChooserFromToValue colorValue = new ColorChooserFromToValue();
        [SerializeField] private SliderFromToValue sliderValue = new SliderFromToValue();
        
        [SerializeField] private FloatReset floatResetValue = new FloatReset();
        [SerializeField] private Vector4Reset vectorResetValue = new Vector4Reset();
        [SerializeField] private ColorChooserReset colorResetValue = new ColorChooserReset();
        [SerializeField] private FloatReset sliderResetValue = new FloatReset();
        
        [SerializeField] private ShaderKeyword shaderKeyword = ShaderKeyword.None;
        [SerializeField] private Ease ease = new Ease();

        private Material material;
        private string key;
        private float duration;

        protected override void Play()
        {
            if (!target.IsValid) {
                return;
            }
            
            timing.Invoke(this, PlayDelayed);
        }

        private void PlayDelayed()
        {
            duration = timing.duration;
            material = target.Value.material;
            
            HandleFloat();
            HandleVector();
            HandleColor();
            HandleRange();

            tween.SetEase(ease.curve);
            tween.SetUpdate(timing.ignoreTimeScale);
        }

        internal override void Stop()
        {
            base.Stop();
            
            if (material == null) {
                return;
            }

            if (key != null) {
                material.DisableKeyword(key);
            }
        }

        private void HandleFloat()
        {
            CalcDuration(floatResetValue);
            Handle(ref Float, floatValue, floatResetValue, 
                () => material.SetFloat(
                    propertyName, floatResetValue.resetValue),
                () => material.DOFloat(
                    floatValue.value, propertyName, duration));

        }

        private void HandleRange()
        {
            CalcDuration(sliderResetValue);
            Handle(ref Range, sliderValue, sliderResetValue,
                () => material.SetFloat(
                    propertyName, sliderResetValue.resetValue),
                () => material.DOFloat(
                    sliderValue.value, propertyName, duration));

        }

        private void HandleColor()
        {
            CalcDuration(colorResetValue);
            Handle(ref Color, colorValue, colorResetValue,
                () => material.SetColor(
                    propertyName, colorResetValue.resetValue.Value),
                () => material.DOBlendableColor(
                    colorValue.value.Value, propertyName, duration));

            key = Keywords[shaderKeyword];

            if (key == null) {
                return;
            }
            
            material.EnableKeyword(key);
            tween.onComplete += () => { material.DisableKeyword(key); };
        }

        private void HandleVector()
        {
            CalcDuration(vectorResetValue);
            Handle(ref Vector, vectorValue, vectorResetValue, 
                () => material.SetVector(
                    propertyName, vectorResetValue.resetValue),
                () => material.DOBlendableVector4(
                    propertyName, vectorValue.value, duration));

        }

        private void CalcDuration<T>(Reset<T> reset)
        {
            if (reset.resetType == ResetType.Yoyo) {
                duration /= 2;
            }
        }

        private void AddIsFrom<T>(FromToValue<T> fromToValue)
        {
            if (fromToValue.isFrom) {
                tween.From();
            }
        }

        private void Loop<T>(Reset<T> reset, Action resetCallback)
        {
            switch (reset.resetType) {
                case ResetType.Yoyo:
                    tween.SetLoops(reset.loop ? -1 : 2, LoopType.Yoyo);
                    break;
                case ResetType.ToValue:
                    tween.SetLoops(reset.loop ? -1 : 1, LoopType.Restart);
                    tween.onComplete += () => resetCallback();
                    break;
            }
        }

        private void Handle<T>(ref string type, FromToValue<T> fromToValue, Reset<T> reset,
            Action callback, Func<Tweener> func)
        {
            if (!propertyType.Equals(type)) {
                return;
            }

            
            tween = func();

            Loop(reset, callback);
            AddIsFrom(fromToValue);
        }
    }
}