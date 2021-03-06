
##Liste des tutoriels
* Theorie par Simon Albou : https://medium.com/@simonalbou/lusage-de-compute-shaders-sur-unity-un-gain-de-performance-violent-54c1b0f72698
* Theorie par Claire Blackshaw : https://www.youtube.com/watch?v=qDk-WIOYUSY et https://www.youtube.com/watch?v=4AVc2YkOGtA
* Tuts-00 basé sur le tutoriel de Vincent Laurent : http://www.vincentlorant.com/blog/compute-shaders/introduction
* Tuts-01 basé sur le tutoriel de Jalmari Ikävalko: https://www.tzaeru.com/site/compute-shader-basics-with-unity/
* Tuts-02 basé sur le tutoriel de Kyle Halladay : http://kylehalladay.com/blog/tutorial/2014/06/27/Compute-Shaders-Are-Nifty.html

##Liste d'exemples
* Flocking : 
* * Juan Francisco Mora Palomino's flocking system : https://github.com/jfranmora/unity-compute-shaders-flocking
* * Jiadong Chen's flocking system : https://github.com/chenjd/Unity-Boids-Behavior-on-GPGPU/
* * Raphael Monnerat's Flocking system : https://github.com/Shinao/Unity-GPU-Boids

##References
* Keijiro flocking system (CPU) : https://github.com/keijiro/Boids
* An approach to parallel processing with Unity : https://software.intel.com/en-us/articles/an-approach-to-parallel-processing-with-unity

##Note sur les Tutoriels
**Theorie par Claire Blackshaw**
(+) Explication claire notament sur la gestion des thread, calcules de strides et concurrence entre thread
(-) Manque d'un exemple concret démontrant les performances

**Theorie par Claire Blackshaw**
(+) Explication claire en video avec les alertes sur les bottleneck
(-) Exemple ne fonctionnant pas sur Unity5.0. Besoins de debug.

**Tutoriel 0 :**
(+) Bonne explication théorique sur fonctionnement des kernel et groupes
(-) Exemples non actionnables : Rendu non fonctionnel

**Tutoriel 1 :**
(+) Explication de A a Z d'un system simple du compute shader au rendu GPU
(-) Quelques erreurs dans les exemples
(-) Un exemples non optimisé tournant à 20FPS. Aucune clef d'amélioration sur ce point

**Tutoriel 2 :**
(+) Explication simple et clair des compute buffer
(-) Manque de détails
(+) Pas d'exemples actionnables
