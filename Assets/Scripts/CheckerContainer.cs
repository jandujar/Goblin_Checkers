using UnityEngine;
using System.Collections;

public class CheckerContainer : MonoBehaviour
{
	[SerializeField] int pieceColor = 1; // 1 = White, 2 = Red
	int boardLocation;
	int pieceType = 1; // 1 = Regular, 2 = King
	public int BoardLocation { get { return boardLocation; } set { boardLocation = value; }}
	public int PieceColor { get { return pieceColor; } set { pieceColor = value; }}
	public int PieceType { get { return pieceType; } set { pieceType = value; }}
}
