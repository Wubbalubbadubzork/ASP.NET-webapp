using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Wubbalubbadubzork.Models {

    public class LoadedScene {

        public string sceneDescription;
        public List<Options> options;

        public LoadedScene(string sceneDescription, List<Options> options) {
            this.sceneDescription = sceneDescription;
            this.options = new List<Options>(options);
        }

    }
}