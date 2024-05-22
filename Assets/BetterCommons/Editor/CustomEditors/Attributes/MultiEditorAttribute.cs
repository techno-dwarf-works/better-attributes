using System;

namespace Better.Commons.EditorAddons.CustomEditors.Attributes
{
    /// <summary>
    ///   <para>Tells an Editor class which run-time type it's an editor for.</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class MultiEditorAttribute : Attribute
    {
        public Type EditorFor { get; }
        public bool EditorForChildClasses { get; }
        /// <summary>
        /// If set true, will disable default Inspector
        /// </summary>
        public bool OverrideDefaultEditor { get; set; }
        public int Order { get; set; }
        
        /// <summary>
        ///   <para>Defines which object type the custom editor class can edit.</para>
        /// </summary>
        /// <param name="inspectedType">Type that this editor can edit.</param>
        /// <param name="editorForChildClasses">If true, child classes of inspectedType will also show this editor. Defaults to false.</param>
        public MultiEditorAttribute(Type inspectedType, bool editorForChildClasses = false)
        {
            EditorFor = inspectedType;
            EditorForChildClasses = editorForChildClasses;
        }
    }
}