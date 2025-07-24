using UnityEngine;

public class Projectiles_explode : Projectiles
{
    [SerializeField] private GameObject _bomb;
    private SpriteRenderer _spriteRenderer;
    private AudioSource _audioSource;

    protected override void Start()
    {
        base.Start();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
    }

    override public void DestroyThis(Collider2D collision)
    {
        if (collision.gameObject.layer == 9 || collision.CompareTag("Player"))
        {
            _audioSource.Play();
            _rb.velocity = Vector3.zero;
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 0f);
            _bomb.SetActive(true);    
        }
       

    }
}
