using UnityEngine;
using System.Collections;
using TMPro;

namespace ProjectYouMustClickYes
{
    public class VertexJitter : MonoBehaviour
    {
        public float angleMultiplier = 1.0f;
        public float speedMultiplier = 1.0f;
        public float curveScale = 1.0f;

        private TMP_Text _textComponent;
        private bool _hasTextChanged;

        private TMP_MeshInfo[] _cachedMeshInfo;
        private Coroutine _jitterCoroutine;

        private struct VertexAnim
        {
            public float angleRange;
            public float angle;
            public float speed;
        }

        void Awake()
        {
            _textComponent = GetComponent<TMP_Text>();
        }

        void OnEnable()
        {
            TMPro_EventManager.TEXT_CHANGED_EVENT.Add(ON_TEXT_CHANGED);
            _jitterCoroutine = StartCoroutine(AnimateVertexColors());
        }

        void OnDisable()
        {
            TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(ON_TEXT_CHANGED);
            if (_jitterCoroutine != null)
            {
                StopCoroutine(_jitterCoroutine);
                _jitterCoroutine = null;
            }

            // 원래 정점으로 복원
            if (_textComponent != null)
            {
                _textComponent.ForceMeshUpdate();
                TMP_TextInfo textInfo = _textComponent.textInfo;

                for (int i = 0; i < textInfo.meshInfo.Length; i++)
                {
                    textInfo.meshInfo[i].mesh.vertices = _cachedMeshInfo[i].vertices;
                    _textComponent.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
                }
            }
        }

        void ON_TEXT_CHANGED(Object obj)
        {
            if (obj == _textComponent)
                _hasTextChanged = true;
        }

        IEnumerator AnimateVertexColors()
        {
            _textComponent.ForceMeshUpdate();
            TMP_TextInfo textInfo = _textComponent.textInfo;

            Matrix4x4 matrix;
            int loopCount = 0;
            _hasTextChanged = true;

            VertexAnim[] vertexAnim = new VertexAnim[1024];
            for (int i = 0; i < vertexAnim.Length; i++)
            {
                vertexAnim[i].angleRange = Random.Range(10f, 25f);
                vertexAnim[i].speed = Random.Range(1f, 3f);
            }

            _cachedMeshInfo = textInfo.CopyMeshInfoVertexData();

            while (true)
            {
                if (_hasTextChanged)
                {
                    _cachedMeshInfo = textInfo.CopyMeshInfoVertexData();
                    _hasTextChanged = false;
                }

                int characterCount = textInfo.characterCount;
                if (characterCount == 0)
                {
                    yield return new WaitForSeconds(0.25f);
                    continue;
                }

                for (int i = 0; i < characterCount; i++)
                {
                    TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
                    if (!charInfo.isVisible)
                        continue;

                    VertexAnim vertAnim = vertexAnim[i];
                    int materialIndex = charInfo.materialReferenceIndex;
                    int vertexIndex = charInfo.vertexIndex;

                    Vector3[] sourceVertices = _cachedMeshInfo[materialIndex].vertices;
                    Vector2 charMidBasline = (sourceVertices[vertexIndex + 0] + sourceVertices[vertexIndex + 2]) / 2;
                    Vector3 offset = charMidBasline;

                    Vector3[] destinationVertices = textInfo.meshInfo[materialIndex].vertices;

                    destinationVertices[vertexIndex + 0] = sourceVertices[vertexIndex + 0] - offset;
                    destinationVertices[vertexIndex + 1] = sourceVertices[vertexIndex + 1] - offset;
                    destinationVertices[vertexIndex + 2] = sourceVertices[vertexIndex + 2] - offset;
                    destinationVertices[vertexIndex + 3] = sourceVertices[vertexIndex + 3] - offset;

                    vertAnim.angle = Mathf.SmoothStep(-vertAnim.angleRange, vertAnim.angleRange, Mathf.PingPong(loopCount / 25f * vertAnim.speed, 1f));
                    Vector3 jitterOffset = new Vector3(Random.Range(-.25f, .25f), Random.Range(-.25f, .25f), 0);

                    matrix = Matrix4x4.TRS(jitterOffset * curveScale, Quaternion.Euler(0, 0, Random.Range(-5f, 5f) * angleMultiplier), Vector3.one);

                    destinationVertices[vertexIndex + 0] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 0]) + offset;
                    destinationVertices[vertexIndex + 1] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 1]) + offset;
                    destinationVertices[vertexIndex + 2] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 2]) + offset;
                    destinationVertices[vertexIndex + 3] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 3]) + offset;

                    vertexAnim[i] = vertAnim;
                }

                for (int i = 0; i < textInfo.meshInfo.Length; i++)
                {
                    textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                    _textComponent.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
                }

                loopCount += 1;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
