using UnityEngine;
using System;
using System.Collections.Generic;
using IA2;
using IA2.FP;
using System.Linq;
using Random = UnityEngine.Random;
using U = ClassExtentions;

[ExecuteInEditMode]
public class Grid : MonoBehaviour
{
	public float x;
	public float z;
	public float cellWidth;
	public float cellHeight;
	public int width;
	public int height;
    public Transform copyGameObject;

    int carreraUpdate = 0;
    int carreraGizmo = 0;

    IEnumerable<int> CountForever()
    {
        return Generate(0, a => a + 1);
    }

    IEnumerable<int> MyRange(int start, int count)
    {
        for (int i = start; i < start + count; i++)
            yield return i;
    }

    int MaxPairs = 0;
    void Update()
    {
        /*carreraUpdate++;

        Debug.Log("CARRERA UPDATE" + carreraUpdate);
        Debug.Log("CARRERA GIZMO " + carreraGizmo);

        if (MaxPairs != 0)
           Debug.Log("MAX PAIRS " + MaxPairs);*/

        
        /*if(Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < 1000; i++)
            {
                var b = Instantiate(copyGameObject);
                b.parent = transform;
                b.position = new Vector3(Random.Range(x, x + width * cellWidth), 0, Random.Range(z, z + height * cellHeight));
                var ge = b.GetComponent<GridEntity>();
                ge.OnMove += UpdateEntity;
                UpdateEntity(ge);
            }
        }*/
    }



	static IEnumerable<Transform> RecursiveWalker(Transform parent)
    {
		foreach(Transform child in parent)
        {
			foreach(Transform grandchild in RecursiveWalker(child))
				yield return grandchild;
			yield return child;
		}
	}

	static IEnumerable<Tuple<int, int, T>> LazyMatrix<T>(T[,] matrix)
    {
		for (int i = 0;  i < matrix.GetLength(0); i++)
			for (int j = 0;  j < matrix.GetLength(1); j++)
				yield return Tuple.Create(i, j, matrix[i, j]);
				//yield return new { x = i, y = j, value =  matrix[i, j]};
				
	}

	Dictionary<GridEntity, Tuple<int, int>> lastPositions;
	HashSet<GridEntity>[,] buckets;

    readonly public Tuple<int, int> Outside = Tuple.Create(-1, -1);
    readonly public GridEntity[] Empty = new GridEntity[0];

	public void UpdateEntity(GridEntity entity)
    {
		var prevPos = lastPositions.ContainsKey(entity) ? lastPositions[entity] : Outside;
		var currPos = PositionInGrid(entity.transform.position);

		//Same pos, no update needed
		if(prevPos.Equals(currPos))
			return;

		//Entity was previously inside the grid and it will move from there
		if(InsideGrid(prevPos))
        {
			buckets[prevPos.Item1, prevPos.Item2].Remove(entity);
		} 

		//Entity is now inside the grid, and just moved from prev cell, add it to the new cell
		if(InsideGrid(currPos))
        {
			buckets[currPos.Item1, currPos.Item2].Add(entity);
			lastPositions[entity] = currPos;
		} 
		else
        {
			lastPositions.Remove(entity);
		}
	}

	public IEnumerable<GridEntity> Query(Vector3 aabbFrom, Vector3 aabbTo, Func<Vector3, bool> filterByPosition)
    {
		var from = new Vector3(Mathf.Min(aabbFrom.x, aabbTo.x), 0, Mathf.Min(aabbFrom.z, aabbTo.z));
		var to  = new Vector3(Mathf.Max(aabbFrom.x, aabbTo.x), 0, Mathf.Max(aabbFrom.z, aabbTo.z));

		var fromCoord = PositionInGrid(from);
		var toCoord = PositionInGrid(to);

        //¡Ojo que clampea a 0,0 el Outside! TODO: Checkear cuando descartar el query si estan del mismo lado
        fromCoord = Tuple.Create(U.Clampi(fromCoord.Item1, 0, width), U.Clampi(fromCoord.Item2, 0, height));
        toCoord = Tuple.Create(U.Clampi(toCoord.Item1, 0, width), U.Clampi(toCoord.Item2, 0, height));

        if (!InsideGrid(fromCoord) && !InsideGrid(toCoord))
			return Empty;

		//TODO p/Alumno: ¿Cómo haría esto con un Aggregate en vez de generar posiciones?
		//TODO p/Alumno: Cambiar por Where/Take

		// Creamos tuplas de cada celda
		var cols = Generate(fromCoord.Item1, x => x + 1)				
			.TakeWhile(x => x < width && x <= toCoord.Item1);

		var rows = Generate(fromCoord.Item2, y => y + 1)
			.TakeWhile(y => y < height && y <= toCoord.Item2);

		var cells = cols.SelectMany(
            col => rows.Select(
                row => Tuple.Create(col, row)
            )
        );

		// Iteramos las que queden dentro del criterio
		return cells
            .SelectMany(cell => buckets[cell.Item1, cell.Item2])
			.Where(e =>
				from.x <= e.transform.position.x && e.transform.position.x <= to.x &&
				from.z <= e.transform.position.z && e.transform.position.z <= to.z)
            .Where(x => filterByPosition(x.transform.position));


	}

	bool InsideGrid(Tuple<int, int> position)
    {
		return 0 <= position.Item1 && position.Item1 < width &&
			0 <= position.Item2 && position.Item2 < height;
	}
    Tuple<int, int> PositionInGrid(Vector3 position)
    {
        return Tuple.Create
        (
            (int)Mathf.Floor((position.x - x) / cellWidth),
            (int)Mathf.Floor((position.z - z) / cellHeight)
        );
    }


    void OnDestroy()
    {
		Debug.Log("DESTROY");
		var ents = RecursiveWalker(transform).Select(x => x.GetComponent<GridEntity>()).Where(x => x != null);
		foreach(var e in ents)
        {
			e.OnMove -= UpdateEntity;
		}
    }

    IEnumerable<T> Generate<T>(T seed, Func<T, T> mutate)
    {
        T accum = seed;
        while(true)
        {
            yield return accum;
            accum = mutate(accum);
        }
    }

    void Awake()
    {
		lastPositions = new Dictionary<GridEntity, Tuple<int, int>>();
		buckets = new HashSet<GridEntity>[width, height];

		for (int i = 0;  i < width; i++)
			for (int j = 0;  j < height; j++)
				buckets[i, j] = new HashSet<GridEntity>();

		var ents = RecursiveWalker(transform)
            .Select(x => x.GetComponent<GridEntity>())
            .Where(x => x != null);

		foreach(var e in ents)
        {
			e.OnMove += UpdateEntity;
			UpdateEntity(e);
		}
	}


	void OnDrawGizmos() {

        MaxPairs = 0;
        carreraGizmo++;

		var cols = Generate(x, curr => curr + cellWidth)
			.Select(col => Tuple.Create(
                new Vector3(col, 0, z),
                new Vector3(col, 0, z + cellHeight * height)
                )
            );

		var rows = Generate(z, curr => curr + cellHeight)
			.Select(row => Tuple.Create(
                new Vector3(x, 0, row),
                new Vector3(x + cellWidth * width, 0, row)
                )
            );

        var allLines =
            cols
                .Take(width + 1)
                .Concat(
                    rows.Take(height + 1)
                    );

        foreach (var line in allLines)
			Gizmos.DrawLine(line.Item1, line.Item2);

		if(buckets != null) {
			var toDraw = LazyMatrix(buckets)
                .Where(t => t.Item3.Count > 0)
                .ToList();

			//Flatten the sphere we're going to draw
			Gizmos.matrix *= Matrix4x4.Scale(Vector3.forward + Vector3.right);
			foreach(var t in toDraw) {
				var ix = t.Item1;
				var iy = t.Item2;
				Gizmos.color = Color.blue;
				Gizmos.DrawWireSphere(
					new Vector3(x, 0, z) +
					new Vector3(
                        (ix + 0.5f) * cellWidth,
                        0,
                        (iy + 0.5f) * cellHeight),
					Mathf.Min(cellWidth, cellHeight) / 2
				);
			}

			Gizmos.color = Color.red;
			Gizmos.matrix = Matrix4x4.identity;
			foreach(var t in toDraw.Where(t => t.Item3.Count >= 2)) {
				var pairs =
                    t.Item3.SelectMany(
                        e1 => t.Item3.Select(
                            e2 => Tuple.Create(e1, e2)
                        )
                    )
                    .Where(pair => pair.Item1 != pair.Item2)
                    ;
                //Debug.Log("PAIRS "+pairs.Count());
                MaxPairs += pairs.Count(); //Math.Max(pairs.Count(), MaxPairs);

				var offset = Vector3.up * 3.0f;
				foreach(var te in pairs) {
					Gizmos.DrawLine(te.Item1.transform.position+offset, te.Item2.transform.position+offset);
					Gizmos.DrawCube(te.Item1.transform.position+offset, Vector3.one);
				}			
			}
		}
	}

	
}
