using UnityEngine;

public class AutoPunchWeapon : MonoBehaviour
{
    [Header("Pengaturan Visual")]
    public Transform weaponVisual; // Masukkan objek gambar senjata ke sini

    [Header("Statistik Senjata")]
    public float attackRange = 5f;       // Jarak deteksi musuh
    public float punchDistance = 1.5f;   // Seberapa jauh senjata maju meninju
    public float punchSpeed = 15f;       // Kecepatan maju
    public float returnSpeed = 10f;      // Kecepatan mundur ke posisi awal
    public float attackCooldown = 0.5f;  // Waktu jeda antar pukulan

    private Vector3 originalLocalPos;
    private Transform targetEnemy;
    private float cooldownTimer;
    public bool isPunching {get; private set; }
    private bool isReturning;

    void Start()
    {
        // Menyimpan posisi awal senjata (jarak dari pivot)
        originalLocalPos = weaponVisual.localPosition;
    }

    void Update()
    {
        // 1. Cari musuh terdekat di setiap frame
        FindNearestEnemy();

        // 2. Putar engsel (pivot) ke arah musuh (jika sedang tidak meninju)
        if (targetEnemy != null && !isPunching && !isReturning)
        {
            RotateTowardsEnemy();
        }

        // 3. Logika Menyerang & Cooldown
        if (cooldownTimer > 0)
        {
            // Mengurangi waktu tunggu. Karena menggunakan Time.deltaTime,
            // pukulan senjata akan ikut melambat/berhenti saat karaktermu diam!
            cooldownTimer -= Time.deltaTime; 
        }
        else if (targetEnemy != null && !isPunching && !isReturning)
        {
            // Waktu tunggu selesai dan ada musuh, mulai meninju!
            isPunching = true;
        }

        // 4. Eksekusi Animasi Pukulan
        HandlePunchAnimation();
    }

    void FindNearestEnemy()
    {
        // Mengumpulkan semua objek yang memiliki Tag "Enemy" (seperti yang kamu buat sebelumnya)
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            
            // Cek apakah musuh ini yang paling dekat dan masuk dalam jangkauan
            if (distanceToEnemy < shortestDistance && distanceToEnemy <= attackRange)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        // Jika ketemu musuh, jadikan target
        if (nearestEnemy != null)
        {
            targetEnemy = nearestEnemy.transform;
        }
        else
        {
            targetEnemy = null;
        }
    }

    void RotateTowardsEnemy()
    {
        // Menghitung arah dari pivot ke musuh
        Vector2 direction = targetEnemy.position - transform.position;
        
        // Rumus matematika untuk mengubah arah vektor menjadi sudut rotasi Z
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void HandlePunchAnimation()
    {
        if (isPunching)
        {
            // Senjata meluncur maju searah sumbu X lokal
            Vector3 targetPunchPos = originalLocalPos + new Vector3(punchDistance, 0, 0);
            weaponVisual.localPosition = Vector3.MoveTowards(weaponVisual.localPosition, targetPunchPos, punchSpeed * Time.deltaTime);

            // Jika sudah mencapai titik maksimal pukulan, tarik kembali
            if (Vector3.Distance(weaponVisual.localPosition, targetPunchPos) < 0.01f)
            {
                isPunching = false;
                isReturning = true;
            }
        }
        else if (isReturning)
        {
            // Senjata ditarik kembali ke posisi asli
            weaponVisual.localPosition = Vector3.MoveTowards(weaponVisual.localPosition, originalLocalPos, returnSpeed * Time.deltaTime);

            // Jika sudah kembali ke posisi awal, mulai hitung mundur cooldown
            if (Vector3.Distance(weaponVisual.localPosition, originalLocalPos) < 0.01f)
            {
                isReturning = false;
                cooldownTimer = attackCooldown; 
            }
        }
    }

    // Fungsi ekstra: Menggambar lingkaran merah di Editor untuk melihat jangkauan sensor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}