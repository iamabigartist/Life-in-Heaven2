using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public struct Gene
{
    static public float move_speed_max = 20, move_speed_min = 10;
    static public float appealing_max = 2.5f, appealing_min = 0.5f;
    static public float charming_max = 0.8f, charming_min = 0.2f;
    static public float seducing_speed_persecond_max = 2f, seducing_speed_persecond_min = 0f;

    public static Dictionary<string, Vector2> limit_dict;
    public Dictionary<string, float> gene_dict;
    public Gender gender;
    public float sum
    {
        get
        {
            float s = 0;
            for (int i = 0; i < gene_dict.Count; i++)
            {
                s += gene_dict.Values.ElementAt(i);
            }
            return s;
        }
    }
    static Gene()
    {
        limit_dict = new Dictionary<string, Vector2>();
        limit_dict["move_speed"] = new Vector2(10f, 20f);
        limit_dict["appealing"] = new Vector2(0.5f, 1.5f);
        limit_dict["charming"] = new Vector2(0.2f, 0.8f);
        limit_dict["seducing_speed_persecond"] = new Vector2(0.1f, 2f);
        Debug.Log(limit_dict["move_speed"]);
    }
    public static Gene Genefromparent(Gene p1, Gene p2)
    {
        Gene m_gene = new Gene();

        #region gender
        if (p1.gender == Gender.Female && p2.gender == Gender.Female)
        {
            m_gene.gender = Gender.Female;
        }
        else if (p1.gender == Gender.Male && p2.gender == Gender.Male)
        {
            m_gene.gender = Random.Range(0, 4) == 0 ? Gender.Female : Gender.Male;
        }
        else
        {
            m_gene.gender = Random.Range(0, 2) == 0 ? Gender.Female : Gender.Male;
        }
        #endregion

        #region genes
        m_gene.gene_dict = new Dictionary<string, float>();
        for (int i = 0; i < limit_dict.Count; i++)
        {
            string gene_name = limit_dict.Keys.ElementAt(i);
            float gene_p1 = p1.gene_dict.Values.ElementAt(i);
            float gene_p2 = p2.gene_dict.Values.ElementAt(i);
            m_gene.gene_dict[gene_name] = Random.Range(0, 2) == 0 ? gene_p1 : gene_p2;
        }
        #endregion 
        return m_gene;
    }
    public static Gene Genefromrandom()
    {
        Gene m_gene = new Gene();

        #region gender
        m_gene.gender = Random.Range(0, 2) == 0 ? Gender.Male : Gender.Female;
        #endregion

        #region genes
        m_gene.gene_dict = new Dictionary<string, float>();
        for (int i = 0; i < limit_dict.Count; i++)
        {
            string gene_name = limit_dict.Keys.ElementAt(i);
            Vector2 gene_limit = limit_dict.Values.ElementAt(i);
            m_gene.gene_dict[gene_name] = Random.Range(0f, 100f);
        }
        #endregion
        return m_gene;
    }
}
