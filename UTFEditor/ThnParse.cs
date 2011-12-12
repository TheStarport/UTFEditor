/*
 * Matt Dean 2011
 * Adapted from LuaParse.cs by Denis Bekman 2009 www.youpvp.com/blog
 --
 * This code is licensed under a Creative Commons Attribution 3.0 United States License.
 * To view a copy of this license, visit http://creativecommons.org/licenses/by/3.0/us/
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Net;

namespace THNEditor
{
    public class ThnParse
    {
        List<string> toks = new List<string>();

        public class Vector
        {
            public float x, y, z;
        };

        public class Entity
        {
            public string entity_name;
          
            public int type;
            public string template_name;
            public int lt_grp;
            public int srt_grp;
            public int usr_flg;
            public int front;
            public int up;
            public Vector ambient;
            public int flags;

            // psysprops
            public bool hs_psp = false;
            public int sparam;

            // spatial props
            public bool has_sp = false;
            public Vector pos;
            public Vector rot1;
            public Vector rot2;
            public Vector rot3;

            // light props
            public bool has_lp = false;
            public Vector lp_ambient;
            public Vector lp_atten;
            public Vector lp_direction;
            public Vector lp_color;
            public Vector lp_specular;
            public Vector lp_diffuse;
            public int lp_type;
            public int lp_theta;
            public float lp_cutoff;
            public int lp_on;
            public int lp_range;

            // user props
            public bool has_up = false;
            public string up_nofog;
            public string up_category;
            public string up_priority;

            // path props
            public bool has_pp = false;
            public string pp_path_data;
            public string pp_path_type;

            // audio props
            public bool has_ap = false;
            public int ap_dmax;
            public int ap_rmix;
            public int ap_dmin;
            public int ap_attenuation;
            public int ap_atout;
            public int ap_pan;
            public int ap_aout;
            public int ap_ain;

            // camera props
            public bool has_cp = false;
            public float cp_farplane;
            public float cp_nearplane;
            public float cp_hvaspect;
            public float cp_fovh;
        }

        public List<Entity> entities = new List<Entity>();

        public String Write()
        {
            StringWriter sw = new StringWriter();

            sw.WriteLine("duration=10000");
            sw.WriteLine();
            sw.WriteLine("entities={");
            
            bool first = true;
            foreach (Entity e in entities)
            {
                if (first)
                    first = false;
                else
                    sw.WriteLine(",");

                sw.WriteLine("{");

                if (e.up != 0)
                {
                    sw.WriteLine("up={0}", e.up);
                    sw.WriteLine(",type={0}", e.type);
                }
                else
                    sw.WriteLine("type={0}", e.type);
                sw.WriteLine(",template_name=\"{0}\"",e.template_name);
                sw.WriteLine(",lt_grp={0}",e.lt_grp);
                sw.WriteLine(",srt_grp={0}",e.srt_grp);
                sw.WriteLine(",usr_flg={0}",e.usr_flg);
                if (e.front != 0)
                    sw.WriteLine(",front={0}", e.front);
                if (e.ambient!=null)
                    sw.WriteLine(",ambient={{ {0:0},{1:0},{2:0} }}", e.ambient.x, e.ambient.y, e.ambient.z);
                if (e.flags!=0)
                    sw.WriteLine(",flags={0}",e.flags);

                if (e.hs_psp)
                {
                    sw.WriteLine(",psysprops={");
                    sw.WriteLine("sparam={0}",e.sparam);
                    sw.WriteLine("}");
                }

                if (e.has_sp)
                {
                    sw.WriteLine(",spatialprops={");
                    sw.WriteLine("orient={{ {{ {0:0.000000},{1:0.000000},{2:0.000000} }}, {{ {3:0.000000},{4:0.000000},{5:0.000000} }}, {{ {6:0.000000},{7:0.000000},{8:0.000000} }} }}, ",
                        e.rot1.x, e.rot1.y, e.rot1.z,
                        e.rot2.x, e.rot2.y, e.rot2.z,
                        e.rot3.x, e.rot3.y, e.rot3.z);
                    sw.WriteLine("pos={{ {0:0.000000},{1:0.000000},{2:0.000000} }}", e.pos.x, e.pos.y, e.pos.z);
                    sw.WriteLine("}");
                }
            
                if (e.has_lp)
                {
                    sw.WriteLine(",lightprops={");
                    sw.WriteLine("ambient={{ {0:0},{1:0},{2:0} }},", e.lp_ambient.x, e.lp_ambient.y, e.lp_ambient.z);
                    sw.WriteLine("atten={{ {0:0.000000},{1:0.000000},{2:0.000000} }},", e.lp_atten.x, e.lp_atten.y, e.lp_atten.z);
                    sw.WriteLine("direction={{ {0:0.000000},{1:0.000000},{2:0.000000} }},", e.lp_direction.x, e.lp_direction.y, e.lp_direction.z);
                    sw.WriteLine("color={{ {0:0},{1:0},{2:0} }},", e.lp_color.x, e.lp_color.y, e.lp_color.z);
                    sw.WriteLine("specular={{ {0:0.000000},{1:0.000000},{2:0.000000} }},", e.lp_specular.x, e.lp_specular.y, e.lp_specular.z);
                    sw.WriteLine("diffuse={{ {0:0.000000},{1:0.000000},{2:0.000000} }},", e.lp_diffuse.x, e.lp_diffuse.y, e.lp_diffuse.z);
                    sw.WriteLine("type={0},",e.lp_type);
                    sw.WriteLine("theta={0},",e.lp_theta);
                    sw.WriteLine("cutoff={0},",e.lp_cutoff);
                    sw.WriteLine("on={0},",e.lp_on);
                    sw.WriteLine("range={0}",e.lp_range);

                    sw.WriteLine("}");
                }

                if (e.has_up)
                {
                    sw.WriteLine(",userprops={");
                    if (e.up_nofog!=null)
                        sw.WriteLine("NoFog=\"{0}\",", e.up_nofog);
                    if (e.up_priority != null)
                        sw.WriteLine("Priority=\"{0}\",", e.up_priority);
                    if (e.up_category != null)
                        sw.WriteLine("category=\"{0}\"", e.up_category);
                    sw.WriteLine("}");
                }

                if (e.has_pp)
                {
                    sw.WriteLine(",pathprop={");
                    sw.WriteLine("path_data=\"{0}\",",e.pp_path_data);
                    sw.WriteLine("path_type=\"{0}\"",e.pp_path_type);
                    sw.WriteLine("}");
                }

                if (e.has_ap)
                {
                    sw.WriteLine(",audioprop={");
                    sw.WriteLine("dmax={0},",e.ap_dmax);
                    sw.WriteLine("rmix={0},",e.ap_rmix);
                    sw.WriteLine("dmin={0},",e.ap_dmin);
                    sw.WriteLine("attenuation={0},",e.ap_attenuation);
                    sw.WriteLine("atout={0},",e.ap_atout);
                    sw.WriteLine("aout={0},",e.ap_aout);
                    sw.WriteLine("ain={0}",e.ap_ain);
                    sw.WriteLine("}");
                }

                sw.WriteLine(",entity_name=\"{0}\"", e.entity_name);
                if (e.has_cp)
                {
                    sw.WriteLine(",cameraprops={");
                    sw.WriteLine("farplane={0},",e.cp_farplane);
                    sw.WriteLine("nearplane={0},",e.cp_nearplane);
                    sw.WriteLine("hvaspect={0},",e.cp_hvaspect);
                    sw.WriteLine("fovh={0}",e.cp_fovh);
                    sw.WriteLine("}");
                }

                sw.WriteLine("}");
            }
            sw.WriteLine("}");
            sw.WriteLine();
            sw.WriteLine("events={{}}");

            return sw.ToString();
        }

        public void Parse(string s)
        {
            string qs = string.Format("({0}[^{0}]*{0})", "\"");
            string[] z = Regex.Split(s, qs + @"|(=)|(,)|(\[)|(\])|(\{)|(\})|(--[^\n\r]*)|(\r\n\r\n)");

            foreach (string tok in z)
            {
                if (tok.Trim().Length != 0 && !tok.StartsWith("--"))
                {
                    toks.Add(tok.Trim());
                }
            }

            // read the duration
            if (!IsToken("duration"))
                throw new Exception("expect 'duration'");
            NextToken(); 
            if (!IsToken("="))
                throw new Exception("expect '='");
            NextToken();
            float duration = GetFloat();

            // Read the entities block
            if (!IsToken("entities"))
                throw new Exception("expect 'duration'");
            NextToken();
            if (!IsToken("="))
                throw new Exception("expect '='");
            NextToken();
            if (!IsToken("{"))
                throw new Exception("expect '{'");
            NextToken();
            ReadEntitiesSection();

            // Read the events block
           
        }

        Vector ReadVectorBlock()
        {
            Vector v = new Vector();
            if (GetToken() != "{")
                throw new Exception("expecting '{': tok = " + toks[0]);
            v.x = GetFloat();
            if (GetToken() != ",")
                throw new Exception("expecting '{': tok = " + toks[0]);
            v.y = GetFloat();
            if (GetToken() != ",")
                throw new Exception("expecting '{': tok = " + toks[0]);
            v.z = GetFloat();
            if (GetToken() != "}")
                throw new Exception("expecting '{': tok = " + toks[0]);
            return v;
        }

        void ReadSpatialProps(Entity e)
        {
            e.has_sp = true;
            if (GetToken() != "{")
                throw new Exception("expecting '{': tok = " + toks[0]);
            if (GetToken() != "orient")
                throw new Exception("expecting 'orient': tok = " + toks[0]);
            if (GetToken() != "=")
                throw new Exception("expecting '=': tok = " + toks[0]);
            if (GetToken() != "{")
                throw new Exception("expecting '{': tok = " + toks[0]);
            e.rot1 = ReadVectorBlock();
            if (GetToken() != ",")
                throw new Exception("expecting ',': tok = " + toks[0]);
            e.rot2 = ReadVectorBlock();
            if (GetToken() != ",")
                throw new Exception("expecting ',': tok = " + toks[0]);
            e.rot3 = ReadVectorBlock();
            if (GetToken() != "}")
                throw new Exception("expecting '}': tok = " + toks[0]);
            if (GetToken() != ",")
                throw new Exception("expecting ',': tok = " + toks[0]);
            if (GetToken() != "pos")
                throw new Exception("expecting 'pos': tok = " + toks[0]);
            if (GetToken() != "=")
                throw new Exception("expecting '=': tok = " + toks[0]);
            e.pos = ReadVectorBlock();
            if (GetToken() != "}")
                throw new Exception("expecting '}': tok = " + toks[0]);
        }

        void ReadLightProps(Entity e)
        {
            e.has_lp = true;
            NextToken();
            while (!IsToken("}"))
            {
                if (IsToken("ambient"))
                {
                    NextToken();
                    if (GetToken() != "=")
                        throw new Exception("expecting '=': tok = " + toks[0]);
                    e.lp_ambient = ReadVectorBlock();
                }
                else if (IsToken("atten"))
                {
                    NextToken();
                    if (GetToken() != "=")
                        throw new Exception("expecting '=': tok = " + toks[0]);
                    e.lp_atten = ReadVectorBlock();
                }
                else if (IsToken("direction"))
                {
                    NextToken();
                    if (GetToken() != "=")
                        throw new Exception("expecting '=': tok = " + toks[0]);
                    e.lp_direction = ReadVectorBlock();
                }
                else if (IsToken("color"))
                {
                    NextToken();
                    if (GetToken() != "=")
                        throw new Exception("expecting '=': tok = " + toks[0]);
                    e.lp_color = ReadVectorBlock();
                }
                else if (IsToken("specular"))
                {
                    NextToken();
                    if (GetToken() != "=")
                        throw new Exception("expecting '=': tok = " + toks[0]);
                    e.lp_specular = ReadVectorBlock();
                }
                else if (IsToken("diffuse"))
                {
                    NextToken();
                    if (GetToken() != "=")
                        throw new Exception("expecting '=': tok = " + toks[0]);
                    e.lp_diffuse = ReadVectorBlock();
                }
                else if (IsToken("theta"))
                {
                    NextToken();
                    if (GetToken() != "=")
                        throw new Exception("expecting '=': tok = " + toks[0]);
                    e.lp_theta = GetNumber();
                }
                else if (IsToken("type"))
                {
                    NextToken();
                    if (GetToken() != "=")
                        throw new Exception("expecting '=': tok = " + toks[0]);
                    e.lp_type = GetNumber();
                }
                else if (IsToken("cutoff"))
                {
                    NextToken();
                    if (GetToken() != "=")
                        throw new Exception("expecting '=': tok = " + toks[0]);
                    e.lp_cutoff = GetFloat();
                }
                else if (IsToken("on"))
                {
                    NextToken();
                    if (GetToken() != "=")
                        throw new Exception("expecting '=': tok = " + toks[0]);
                    e.lp_on = GetNumber();
                }
                else if (IsToken("range"))
                {
                    NextToken();
                    if (GetToken() != "=")
                        throw new Exception("expecting '=': tok = " + toks[0]);
                    e.lp_range = GetNumber();
                }
                else
                {
                    throw new Exception("unrecognised token: tok = " + toks[0]);
                }
                if (IsToken(","))
                    NextToken();
            }
            NextToken();
        }

        void ReadUserProps(Entity e)
        {
            e.has_up = true;
            NextToken();
            while (!IsToken("}"))
            {
                if (IsToken("NoFog"))
                {
                    NextToken();
                    if (GetToken() != "=")
                        throw new Exception("expecting '=': tok = " + toks[0]);
                    e.up_nofog = GetString();
                }
                else if (IsToken("Priority"))
                {
                    NextToken();
                    if (GetToken() != "=")
                        throw new Exception("expecting '=': tok = " + toks[0]);
                    e.up_priority = GetString();
                }
                else if (IsToken("category"))
                {
                    NextToken();
                    if (GetToken() != "=")
                        throw new Exception("expecting '=': tok = " + toks[0]);
                    e.up_category = GetString();
                }
                else
                {
                    throw new Exception("unrecognised token: tok = " + toks[0]);
                }
                if (IsToken(","))
                    NextToken();
            }
            NextToken();
        }

        void ReadPathProps(Entity e)
        {
            e.has_pp = true;
            NextToken();
            while (!IsToken("}"))
            {
                if (IsToken("path_data"))
                {
                    NextToken();
                    if (GetToken() != "=")
                        throw new Exception("expecting '=': tok = " + toks[0]);
                    e.pp_path_data = GetString();
                }
                else if (IsToken("path_type"))
                {
                    NextToken();
                    if (GetToken() != "=")
                        throw new Exception("expecting '=': tok = " + toks[0]);
                    e.pp_path_type = GetString();
                }
                else
                {
                    throw new Exception("unrecognised token: tok = " + toks[0]);
                }
                if (IsToken(","))
                    NextToken();
            }
            NextToken();
        }

        void ReadAudioProps(Entity e)
        {
            e.has_ap = true;
            NextToken();
            while (!IsToken("}"))
            {
                if (IsToken("dmax"))
                {
                    NextToken();
                    if (GetToken() != "=")
                        throw new Exception("expecting '=': tok = " + toks[0]);
                    e.ap_dmax = GetNumber();
                }
                else if (IsToken("rmix"))
                {
                    NextToken();
                    if (GetToken() != "=")
                        throw new Exception("expecting '=': tok = " + toks[0]);
                    e.ap_rmix = GetNumber();
                }
                else if (IsToken("dmin"))
                {
                    NextToken();
                    if (GetToken() != "=")
                        throw new Exception("expecting '=': tok = " + toks[0]);
                    e.ap_dmin = GetNumber();
                }
                else if (IsToken("attenuation"))
                {
                    NextToken();
                    if (GetToken() != "=")
                        throw new Exception("expecting '=': tok = " + toks[0]);
                    e.ap_attenuation = GetNumber();
                }
                else if (IsToken("atout"))
                {
                    NextToken();
                    if (GetToken() != "=")
                        throw new Exception("expecting '=': tok = " + toks[0]);
                    e.ap_atout = GetNumber();
                }
                else if (IsToken("pan"))
                {
                    NextToken();
                    if (GetToken() != "=")
                        throw new Exception("expecting '=': tok = " + toks[0]);
                    e.ap_pan = GetNumber();
                }
                else if (IsToken("aout"))
                {
                    NextToken();
                    if (GetToken() != "=")
                        throw new Exception("expecting '=': tok = " + toks[0]);
                    e.ap_aout = GetNumber();
                }
                else if (IsToken("ain"))
                {
                    NextToken();
                    if (GetToken() != "=")
                        throw new Exception("expecting '=': tok = " + toks[0]);
                    e.ap_ain = GetNumber();
                }
                else
                {
                    throw new Exception("unrecognised token: tok = " + toks[0]);
                }
                if (IsToken(","))
                    NextToken();
            }
            NextToken();
        }

        void ReadCameraProps(Entity e)
        {
            e.has_cp = true;
            NextToken();
            while (!IsToken("}"))
            {
                if (IsToken("farplane"))
                {
                    NextToken();
                    if (GetToken() != "=")
                        throw new Exception("expecting '=': tok = " + toks[0]);
                    e.cp_farplane = GetFloat();
                }
                else if (IsToken("nearplane"))
                {
                    NextToken();
                    if (GetToken() != "=")
                        throw new Exception("expecting '=': tok = " + toks[0]);
                    e.cp_nearplane = GetFloat();
                }
                else if (IsToken("hvaspect"))
                {
                    NextToken();
                    if (GetToken() != "=")
                        throw new Exception("expecting '=': tok = " + toks[0]);
                    e.cp_hvaspect = GetFloat();
                }
                else if (IsToken("fovh"))
                {
                    NextToken();
                    if (GetToken() != "=")
                        throw new Exception("expecting '=': tok = " + toks[0]);
                    e.cp_fovh = GetFloat();
                }
                else
                {
                    throw new Exception("unrecognised token: tok = " + toks[0]);
                }
                if (IsToken(","))
                    NextToken();
            }
            NextToken();
        }

        void ReadEntityBlock(Entity e)
        {
            while (!IsToken("}"))
            {
                if (IsToken("spatialprops"))
                {
                    NextToken();
                    if (GetToken() != "=")
                        throw new Exception("expecting '=': tok = " + toks[0]);
                    ReadSpatialProps(e);
                }
                else if (IsToken("lightprops"))
                {
                    NextToken();
                    if (GetToken() != "=")
                        throw new Exception("expecting '=': tok = " + toks[0]);
                    ReadLightProps(e);
                }
                else if (IsToken("entity_name"))
                {
                    NextToken();
                    if (GetToken() != "=")
                        throw new Exception("expecting '=': tok = " + toks[0]);
                    e.entity_name = GetString();
                }
                else if (IsToken("template_name"))
                {
                    NextToken();
                    if (GetToken() != "=")
                        throw new Exception("expecting '=': tok = " + toks[0]);
                    e.template_name = GetString();
                }
                else if (IsToken("usr_flg"))
                {
                    NextToken();
                    if (GetToken() != "=")
                        throw new Exception("expecting '=': tok = " + toks[0]);
                    e.usr_flg = GetNumber();
                }
                else if (IsToken("type"))
                {
                    NextToken();
                    if (GetToken() != "=")
                        throw new Exception("expecting '=': tok = " + toks[0]);
                    e.type = GetNumber();
                }
                else if (IsToken("srt_grp"))
                {
                    NextToken();
                    if (GetToken() != "=")
                        throw new Exception("expecting '=': tok = " + toks[0]);
                    e.srt_grp = GetNumber();
                }
                else if (IsToken("lt_grp"))
                {
                    NextToken();
                    if (GetToken() != "=")
                        throw new Exception("expecting '=': tok = " + toks[0]);
                    e.lt_grp = GetNumber();
                }
                else if (IsToken("front"))
                {
                    NextToken();
                    if (GetToken() != "=")
                        throw new Exception("expecting '=': tok = " + toks[0]);
                    e.front = GetNumber();
                }
                else if (IsToken("up"))
                {
                    NextToken();
                    if (GetToken() != "=")
                        throw new Exception("expecting '=': tok = " + toks[0]);
                    e.up = GetNumber();
                }
                else if (IsToken("ambient"))
                {
                    NextToken();
                    if (GetToken() != "=")
                        throw new Exception("expecting '=': tok = " + toks[0]);
                    e.ambient = ReadVectorBlock();
                }
                else if (IsToken("flags"))
                {
                    NextToken();
                    if (GetToken() != "=")
                        throw new Exception("expecting '=': tok = " + toks[0]);
                    e.flags = GetNumber();
                }
                else if (IsToken("psysprops"))
                {
                    NextToken();
                    if (GetToken() != "=")
                        throw new Exception("expecting '=': tok = " + toks[0]);
                    if (GetToken() != "{")
                        throw new Exception("expecting '{': tok = " + toks[0]);
                    if (GetToken() != "sparam")
                        throw new Exception("expecting 'sparam': tok = " + toks[0]);
                    if (GetToken() != "=")
                        throw new Exception("expecting '=': tok = " + toks[0]);
                    e.sparam = GetNumber();
                    if (GetToken() != "}")
                        throw new Exception("expecting '}': tok = " + toks[0]);
                }
                else if (IsToken("userprops"))
                {
                    NextToken();
                    if (GetToken() != "=")
                        throw new Exception("expecting '=': tok = " + toks[0]);
                    ReadUserProps(e);
                }
                else if (IsToken("pathprops"))
                {
                    NextToken();
                    if (GetToken() != "=")
                        throw new Exception("expecting '=': tok = " + toks[0]);
                    ReadPathProps(e);
                }
                else if (IsToken("audioprops"))
                {
                    NextToken();
                    if (GetToken() != "=")
                        throw new Exception("expecting '=': tok = " + toks[0]);
                    ReadAudioProps(e);
                }
                else if (IsToken("cameraprops"))
                {
                    NextToken();
                    if (GetToken() != "=")
                        throw new Exception("expecting '=': tok = " + toks[0]);
                    ReadCameraProps(e);
                }
                else
                {
                    //NextToken();
                    throw new Exception("unrecognised token: tok = " + toks[0]);
                }
                if (IsToken(","))
                    NextToken();
            }
            NextToken();
        }

        void ReadEntitiesSection()
        {
            while (!IsToken("}"))
            {
                if (IsToken("{"))
                {
                    NextToken();
                    Entity e = new Entity();
                    ReadEntityBlock(e);
                    entities.Add(e);
                }
                if (IsToken(","))
                    NextToken();
            }
        }

        

        protected bool IsLiteral
        {
            get
            {
                return Regex.IsMatch(toks[0], "^[a-zA-Z]+[0-9a-zA-Z_]*");
            }
        }
        protected bool IsString
        {
            get
            {
                return Regex.IsMatch(toks[0], "^\"([^\"]*)\"");
            }
        }
        protected bool IsNumber
        {
            get
            {
                return Regex.IsMatch(toks[0], @"^\d+");
            }
        }
        protected bool IsFloat
        {
            get
            {
                return Regex.IsMatch(toks[0], @"^\d*\.\d+");
            }
        }
        protected string GetToken()
        {
            string v = toks[0];
            toks.RemoveAt(0);
            return v;
        }
        protected string GetString()
        {
            Match m = Regex.Match(toks[0], "^\"([^\"]*)\"");
            string v = m.Groups[1].Captures[0].Value;
            toks.RemoveAt(0);
            return v;
        }
        protected int GetNumber()
        {
            int v = Convert.ToInt32(toks[0]);
            toks.RemoveAt(0);
            return v;
        }
        protected float GetFloat()
        {
            float v = Convert.ToSingle(toks[0]);
            toks.RemoveAt(0);
            return v;
        }
        protected void NextToken()
        {
            toks.RemoveAt(0);
        }
        protected bool IsToken(string s)
        {
            return toks[0] == s;
        }
    }
}
