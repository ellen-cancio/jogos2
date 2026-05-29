using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour {
    public TextMeshProUGUI textoScore, textoAmmo;
    public GameObject textoGameOver;
    public int score = 0, municao = 30;
    public float tempo = 60f;
    private bool jogoAtivo = true;

    void Start() { AtualizarTextos(); }
    void Update() {
        if (jogoAtivo) {
            tempo -= Time.deltaTime;
            AtualizarTextos();
            if (tempo <= 0 || municao <= 0) FimDeJogo();
        }
    }
    public void AdicionarScore(int pontos) {
        if (jogoAtivo) { score += pontos; AtualizarTextos(); }
    }
    public void GastarMunicao() {
        if (jogoAtivo && municao > 0) { municao--; AtualizarTextos(); }
    }
    void AtualizarTextos() {
        textoScore.text = "Score: " + score;
        textoAmmo.text = "Ammo: " + municao + " | Tempo: " + Mathf.RoundToInt(tempo);
    }
    void FimDeJogo() {
        jogoAtivo = false; tempo = 0; textoGameOver.SetActive(true);
    }
}