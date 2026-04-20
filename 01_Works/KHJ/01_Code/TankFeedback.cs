using UnityEngine;

public class TankFeedback : MonoBehaviour
{
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    private void Start()
    {
        var renderer = GetComponent<Renderer>();
        if (renderer == null) return;

        var mat = renderer.material; // 오브젝트 전용 Material 가져오기
        mat.EnableKeyword("_EMISSION"); // Emission 활성화
        mat.SetColor(EmissionColor, Color.white); // 즉시 하얀색으로 변경
    }
}
