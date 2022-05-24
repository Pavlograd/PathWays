using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnElement : MonoBehaviour
{
    [SerializeField] RuntimeAnimatorController animatorController;
    [SerializeField] SFXManager _SFXManager;

    // Start is called before the first frame update
    void Start()
    {
        Animator animator = gameObject.AddComponent(typeof(Animator)) as Animator;

        animator.runtimeAnimatorController = animatorController;

        Invoke("DestroySpawnThings", animator.runtimeAnimatorController.animationClips[0].length);

        _SFXManager.ChangeState("Plop");
    }

    void DestroySpawnThings()
    {
        Destroy(GetComponent<Animator>());
        transform.localScale = Vector3.one;
        Destroy(_SFXManager.gameObject);
        Destroy(this);
    }
}
