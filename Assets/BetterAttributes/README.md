# Better Attributes

[![openupm](https://img.shields.io/npm/v/com.uurha.betterattributes?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.uurha.betterattributes/)

![image](https://user-images.githubusercontent.com/22265817/181865901-35fea6f6-0b6e-4246-9df5-99e13cb5ed0f.png)

This package provides following features:

## Select Dropdown

This package contains base class for drawer with better dropdown.<br>
Better dropdown contains:
1. Search
2. Fast navigation
3. Grouping

## Select Attribute

Provides possibility to select interface implementation, enum value with better dropdown in Unity Inspector.<br>
Also supports **_flag_** enums.

Usage:

```c#
[Select] [SerializeReference]
private ISomeInterface someInterface;

[Select] [SerializeReference]
private SomeAbstractClass someAbstractClass;

[Select(typeof(SomeAbstractClass))] [SerializeReference]
private List<SomeAbstractClass> someAbstractClasses;

[Select(typeof(ISomeInterface))] [SerializeReference]
private List<ISomeInterface> someInterfaces;

[Select] [SerializeField]
private KeyCode keyCode;
```

## Dropdown Attribute

Similar to Select Attribute, but allows to define your own collection of selecting.

Usage:

```c#
[Dropdown("Method()")] [SerializeField]
private List<int> someInts;

[Dropdown("Method()")] [SerializeField]
private List<int> someInts;
```

Supports retrieving information from other classes, to use this option start `selectorName` with `r:`.

Usage:

```c#
[Dropdown("r:OtherClass.StaticFieldOrProperty")] [SerializeField]
private List<int> someInts;

[Dropdown("r:OtherClass.StaticMethod()")] [SerializeField]
private List<int> someInts;

[Dropdown("r:Singleton.Instance.Method()")] [SerializeField]
private List<int> someInts;

[Dropdown("r:Singleton.Instance.Method()")] [SerializeField]
private List<int> someInts;
```

## Preview

Provides possibility to see object preview by clicking into the field in Unity Inspector.<br>
Supports preview for **_scene objects_** and **_prefabs_** object as well as **_textures_** and **_sprites_**.

Usage:

```c#
[Preview] [SerializeField]
private Sprite sprite;

[Preview] [SerializeField]
private SomeMonobehaviour someMonobehaviour;
```

## Read Only Field

Provides possibility to disable modification of fields in Unity Inspector but keep it displayed.

Usage:

```c#
[ReadOnlyField] [SerializeField] 
private SomeClass someClass;

[ReadOnlyField] [SerializeField] 
private float someFloat;

[ReadOnlyField] [TextArea(5, 10)] [SerializeField] 
private string someString;
```

## Icon Header

Provides possibility to Draw fullsized texture about field in Unity Inspector.
To get texture guid open TextureImport window and right open context menu -> Conver To IconHeaderAttribute.

Usage:

```c#
[IconHeader("TEXTURE_GUID")] [SerializeField]
private string oldName;
```

## Draw Inspector

Provides possibility to Draw fullsized Inspector below field with type inheretex from UnityEngine.Object.

Usage:

```c#
[DrawInspector] [SerializeField]
private SomeScriptable scriptable;
```

## Rename Field

Provides possibility to rename label in Unity Inspector.

Usage:

```c#
[RenameField("New Name")] [SerializeField]
private string oldName;
```

## Gizmo / Gizmo Local

Provides possibility to set value for Vector3/Vector2/Quaternion/Bounds from scene view by dragging handles.<br>
`[GizmoLocal]` works only into `MonoBehaviour` Unity Inspector.

Usage:

```c#
[Gizmo]
[SerializeField] private Bounds bounds;
        
[Gizmo]
[SerializeField] private Vector3 vector3;
        
[Gizmo]
[SerializeField] private Quaternion quaternion;

[GizmoLocal]
[SerializeField] private Bounds boundsLocal;
        
[GizmoLocal]
[SerializeField] private Vector3 vector3Local;
        
[GizmoLocal]
[SerializeField] private Quaternion quaternionLocal;
```

## Editor Buttons

Provides possibility to display button for method in Unity Inspector.

Usages:

```c#
///Default usage of attribute.
[EditorButton]
private void SomeMethod()
{
//Some code.
}

///This button will call method with predefined parameters. 
///When invokeParams not specified will call with null.
[EditorButton(invokeParams: 10f)]
private void SomeMethod(float floatValue)
{
//Some code.
}

///This button will call method with predefined parameters. 
///When invokeParams not specified will call with null.
[EditorButton(invokeParams: new object[] { 10f, 10 })]
private void SomeMethod(float floatValue, int intValue)
{
//Some code.
}

/// This button will be in the same row with button for SomeMethod2.
/// But will be in the second position.
/// When captureGroup not specified each button placed in separate row.
/// When priority not specified buttons in one row sorted by order in code.
[EditorButton(captureGroup: 1, priority: 2)]
private void SomeMethod1()
{
//Some code.
}

[EditorButton(captureGroup: 1, priority: 1)]
private void SomeMethod2()
{
//Some code.
}

/// This button will have name "Some Cool Button".
/// When displayName not specified or null/empty/whitespace button 
/// will have name same as method.
[EditorButton(displayName: "Some Cool Button")]
private void SomeMethod()
{
//Some code
}
```

You can check constructors for `EditorButtonAttribute` there more specific options.

## Install
[How to install](https://github.com/uurha/BetterPluginCollection/wiki/How-to-install)
