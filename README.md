# EscapeSpace
Escape Space est un projet étudiant solo d'un jeu de résolution d'énigmes à la première personne, basé sur des capacités d'intelligence artificielle. Il s'agit d'un projet développé avec Unity (version 6000.0.38f1)

## Unity Sentis
L'intelligence artificielle est ajoutée grâce à [Unity Sentis](https://unity.com/products/sentis) (version 2.1.2). Plusieurs modèles sont disponibles sur le site d'[Hugging Face](https://huggingface.co/models?library=onnx,unity-sentis&sort=trending) et voici ceux utilisés dans le jeu :

### [Détection d'écriture humaine](https://huggingface.co/unity/sentis-MNIST-12)
Ce modèle permet de détecter un chiffre dessiné par un utilisateur. Pour ce faire, j'utilise un matériel sur lequel le joueur peut dessiner en utilisant sa souris. Je récupère ensuite la texture de ce matériel et le modèle d'IA la traite et retourne sa prédiction. Les résultats sont souvent satisfaisants. Quelques fois, le modèle peut se tromper, mais si on dessine avec précision le chiffre, tout devrait se dérouler comme prévu. Le joueur utilise donc ce système pour ouvrir des portes. Un code est caché et celui-ci doit le trouver et l'écrire dans le panneau. Il s'agit de codes à trois chiffres, et chacun doit être entré individuellement. Les lumières au dessus du panneau donnent une rétroaction quant au code entré.

![image](https://github.com/user-attachments/assets/755c579d-8254-4ab1-b3e1-7db0510fcd07)

### [Détection d'objet](https://github.com/HoloLabInc/HoloLabDnnPackages/tree/main)

Ce modèle permet de détecter un objet selon une texture donnée. Son utilisation dans le jeu va comme suit : une caméra filme en continue l'emplacement où l'objet sera placé. Je récupère ensuite une seule image depuis le rendu de la caméra et je la converti en texture. Celle-ci est alors traitée par le modèle et celui-ci me retourne sa prédiction. Le joueur doit apporter les objets correspondants à la demande et alors il pourra avancer. Ces objets sont déterminer aléatoirement et sont décrit avec des devinettes afin de rendre la tâche plus difficile. Le défi de ce modèle est qu'il n'est pas vraiment conçu pour des objets 3D dans un jeu vidéo, mais plutôt pour des images d'objets réels. C'est pourquoi j'ai dû m'assurer de sélectionner minutieusement les objets.

![image](https://github.com/user-attachments/assets/56a57749-02f5-4641-948a-74389c84501e)

### ["Text-to-speech"](https://github.com/Macoron/piper.unity)

Pour donner un peu plus de vie et de credibilité à l'IA, j'ai également intégrer un modèle de "Text-To-Speech" au jeu. Celui-ci prend forme grâce à l'IA du vaisseau dans lequel le joueur se trouve. Celle-ci le guidera au travers de son parcours. Il suffit de lui fournir une variable de type "string" et le [modèle](https://huggingface.co/rhasspy/piper-voices/tree/main/fr/fr_FR/siwis/medium) s'occupe de l'audio. J'utilise cela pour un peu de narration, mais également pour la section de la détection d'objets afin de guider le joueur.

![image](https://github.com/user-attachments/assets/d9ca206e-27cd-4088-bdb0-53259dc13300)

## Autres fonctionnalités

### Système de notes
Au travers de son parcours, le joueur trouvera différentes notes provenant d'un mystérieux allié qui souhaite le guider dans son aventure. Ces notes comportent des indices sur les énigmes à résoudre, alors lisez les biens.

### Système de génération de positions aléatoires
Ce système est utilisé afin d'éviter la répétition de code au travers de plusieurs essais du jeu. Plusieurs positions sont déterminées à l'avance et un script s'occupe ensuite de générer aléatoirement un objet à une position libre, tout en évitant de placer 2 objets au même endroit. 

### Intéraction
Un système d'intéraction modulable est également en place afin d'ajouter facilement et rapidement des intéractions avec des objets. La plupart des interactions de ce système se font avec la touche 'E'. Cependant, un autre script gère un autre type d'intéraction. Certains objets peuvent être pris par le joueur à l'aide du clic gauche. Un curseur apparaît lorsque l'interaction est possible.

## Assets utilisés
- https://assetstore.unity.com/packages/3d/props/free-sport-balls-293937
- https://assetstore.unity.com/packages/tools/physics/grab-it-105735
- https://assetstore.unity.com/packages/3d/props/mobile-power-ups-free-vol-1-36106
- https://assetstore.unity.com/packages/3d/props/interior/office-pack-free-258600
- https://assetstore.unity.com/packages/essentials/starter-assets-firstperson-updates-in-new-charactercontroller-pa-196525
- https://assetstore.unity.com/packages/3d/props/tools/survival-game-tools-139872

## Audio
Tous les sons ont été pris sur https://freesound.org/. Les musiques ont été prises sur https://pixabay.com/.





