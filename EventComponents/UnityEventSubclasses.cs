﻿using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class StringEvent : UnityEvent<string> {}

[System.Serializable]
public class FloatEvent : UnityEvent<float> {}

[System.Serializable]
public class IntEvent : UnityEvent<int> {}

[System.Serializable]
public class GameObjectEvent : UnityEvent<GameObject> {}

[System.Serializable]
public class Vector3Event : UnityEvent<Vector3> {}