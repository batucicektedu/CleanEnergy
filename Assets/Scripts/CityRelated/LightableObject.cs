using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class LightableObject : MonoBehaviour
{
    public Color litUpColor = Color.yellow;

    private MeshRenderer _renderer;
    private List<MeshRenderer> _multipleLevelRenderers;

    private LightableObjectsManager _lightableObjectsManager;

    [Tooltip("Only set on one of the twins")]
    public LightableObject twinBuilding;

    public bool multipleLevelBuilding;

    public bool halfLit;

    public bool LitUp { private set; get; }

    protected virtual void Awake()
    {
        if (multipleLevelBuilding)
        {
            _multipleLevelRenderers = GetComponentsInChildren<MeshRenderer>().ToList();
        }
        else
        {
            _renderer = GetComponentInChildren<MeshRenderer>();
        }
        
        if (!TryGetComponent(out _lightableObjectsManager))
        {
            _lightableObjectsManager = GetComponentInParent<LightableObjectsManager>();  
        }
    }

    public virtual void LightUp(out bool halfLitInfo)
    {
        halfLitInfo = false;
        
        //_renderer.material = GameDataManager.Instance.LitUpMat;
        if (multipleLevelBuilding)
        {
            if (halfLit)
            {
                for (int i = 0; i < _multipleLevelRenderers.Count/2; i++)
                {
                    _multipleLevelRenderers[i].materials[1].color = litUpColor;
                }
                
                LitUp = true;
                _lightableObjectsManager.LightableObjectLitUp();

                if (GetComponentInParent<LightableObject_ChildLighter>())
                {
                    Debug.Log("make childlighter litUp true", gameObject);
                    GetComponentInParent<LightableObject_ChildLighter>().LitUp = true;
                }
                
                halfLit = false;
                halfLitInfo = halfLit;
            }
            else
            {
                for (int i = _multipleLevelRenderers.Count/2; i < _multipleLevelRenderers.Count; i++)
                {
                    _multipleLevelRenderers[i].materials[1].color = litUpColor;
                }
                
                halfLit = true;
                halfLitInfo = halfLit;
            }
        }
        else 
        {
            _renderer.materials[1].color = litUpColor;
            LitUp = true;
            _lightableObjectsManager.LightableObjectLitUp();
        }
        
        if (twinBuilding != null)
        {
            twinBuilding.LightUp();
        }
    }
    
    public virtual void LightUp()
    {
        //_renderer.material = GameDataManager.Instance.LitUpMat;
        if (multipleLevelBuilding)
        {
            if (halfLit)
            {
                for (int i = 0; i < _multipleLevelRenderers.Count/2; i++)
                {
                    _multipleLevelRenderers[i].materials[1].color = litUpColor;
                }
                
                LitUp = true;
                _lightableObjectsManager.LightableObjectLitUp();

                if (GetComponentInParent<LightableObject_ChildLighter>())
                {
                    Debug.Log("make childlighter litUp true", gameObject);
                    GetComponentInParent<LightableObject_ChildLighter>().LitUp = true;
                }
                
                halfLit = false;
            }
            else
            {
                for (int i = _multipleLevelRenderers.Count/2; i < _multipleLevelRenderers.Count; i++)
                {
                    _multipleLevelRenderers[i].materials[1].color = litUpColor;
                }

                halfLit = true;
            }
        }
        else 
        {
            _renderer.materials[1].color = litUpColor;
            LitUp = true;
            _lightableObjectsManager.LightableObjectLitUp();
        }
        
        if (twinBuilding != null)
        {
            twinBuilding.LightUp();
        }
    }
}
